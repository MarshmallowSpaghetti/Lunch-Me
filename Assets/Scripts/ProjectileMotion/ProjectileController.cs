using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Transform launchPoint;

    public ProjectileArc projectileArc;

    private float m_currentSpeed;
    private float m_currentRadian;

    private Vector3 m_targetPoint;

    [SerializeField]
    float cooldown = 0.1f;
    private float lastShotTime;

    enum LaunchType
    {
        useInitialAngle,
        useInitialSpeed,
        useBothAngleAndSpeed
    }

    [SerializeField]
    private float m_initialDegree = 45;
    [SerializeField]
    private float m_initialSpeed = 15;
    [SerializeField]
    private bool m_useLowAngle = true;

    public float minDegree = 0;
    public float maxDegree = 60;
    public float minSpeed = 1;
    public float maxSpeed = 15;
    private float m_dynamicMaxSpeed = 15;
    public float chargeSensitivity = 1f;

    [SerializeField]
    private LaunchType type = LaunchType.useInitialAngle;

    private bool m_isEnabled = false;

    public float DynamicMaxSpeed
    {
        get
        {
            return m_dynamicMaxSpeed;
        }

        set
        {
            m_dynamicMaxSpeed = value;
        }
    }

    public float InitDegree
    {
        get
        {
            return m_initialDegree;
        }

        set
        {
            value = Mathf.Clamp(value, minDegree, maxDegree);
            m_initialDegree = value;
        }
    }

    private void Start()
    {
        Physics.gravity = new Vector3(0f, -30f, 0f);
        //print("gravity " + Physics.gravity);

        DynamicMaxSpeed = m_initialSpeed;
    }

    private void Update()
    {
        if (m_isEnabled == false)
            return;

        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        if (type == LaunchType.useInitialAngle)
            SetTargetWithAngle(MouseInput.Instance.MousePos, m_initialDegree);
        else if (type == LaunchType.useInitialSpeed)
            SetTargetWithSpeed(MouseInput.Instance.MousePos, m_initialSpeed, m_useLowAngle);
        else if (type == LaunchType.useBothAngleAndSpeed)
            SetTargetWithBothAngleAndSpeed(MouseInput.Instance.MousePos, m_initialDegree, m_initialSpeed);
    }

    public void SetEnable(bool _isEnable)
    {
        m_isEnabled = _isEnable;
        projectileArc.gameObject.SetActive(_isEnable);
    }

    public void StartCharge()
    {
        m_initialSpeed = minSpeed;
    }

    public void StopChargeAndLaunch()
    {
        Launch();

        m_initialSpeed = minSpeed;
        // To clear the old line before next time it apears
        projectileArc.GetComponent<LineRenderer>().positionCount = 0;
    }

    public void SetLaunchChargeTime(float _time)
    {
        m_initialSpeed = Mathf.Lerp(m_initialSpeed, DynamicMaxSpeed, chargeSensitivity * Time.deltaTime);

        UpdateDynamicalMaxSpeed();
    }

    private void UpdateDynamicalMaxSpeed()
    {
        Vector3 direction2D =
            (MouseInput.Instance.MousePos - launchPoint.position).SetY(0);
        Vector3 extend = Mathf.Max(direction2D.magnitude * 0.5f, 1) * direction2D.normalized;

        float maxSpeed =
            CalRequiredSpeedToReachTarget(MouseInput.Instance.MousePos + extend, InitDegree);

        //print("max speed " + maxSpeed);
        DynamicMaxSpeed = maxSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(m_targetPoint, 0.2f);
    }

    public void SetTargetWithAngle(Vector3 _target, float _degree)
    {
        m_currentRadian = _degree * Mathf.Deg2Rad;

        m_targetPoint = _target;
        //GizmosHelper.DrawBox(point, Vector3.one * 0.2f, Color.yellow);
        Vector3 direction = _target - launchPoint.position;

        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = direction.magnitude;

        m_currentSpeed = ProjectileMath.CalculateLaunchSpeed(distance, yOffset, Physics.gravity.magnitude, _degree * Mathf.Deg2Rad);

        projectileArc.UpdateArc(m_currentSpeed, distance, Physics.gravity.magnitude, m_currentRadian, direction, true);
        SetLaunchPoint(direction, m_currentRadian * Mathf.Rad2Deg);
    }

    public float CalRequiredSpeedToReachTarget(Vector3 _target, float _degree)
    {
        Vector3 direction = _target - launchPoint.position;

        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = direction.magnitude;

        return ProjectileMath.CalculateLaunchSpeed(distance, yOffset, Physics.gravity.magnitude, _degree * Mathf.Deg2Rad);
    }

    public void SetTargetWithSpeed(Vector3 _target, float _speed, bool _useLowAngle)
    {
        m_currentSpeed = _speed;

        Vector3 direction = _target - launchPoint.position;
        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = direction.magnitude;

        float angle0, angle1;
        bool targetInRange = ProjectileMath.CalculateLaunchAngle(_speed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);

        if (targetInRange)
            m_currentRadian = _useLowAngle ? angle1 : angle0;
        
        projectileArc.UpdateArc(_speed, distance, Physics.gravity.magnitude, m_currentRadian, direction, targetInRange);
        SetLaunchPoint(direction, m_currentRadian * Mathf.Rad2Deg);
    }

    public void SetTargetWithBothAngleAndSpeed(Vector3 _target, float _degree, float _speed)
    {
        m_currentRadian = _degree * Mathf.Deg2Rad;
        m_currentSpeed = _speed;

        Vector3 direction = _target - launchPoint.position;
        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = ProjectileMath.CalculateLandDistance(launchPoint.position.y, Physics.gravity.magnitude, _degree, _speed);

        projectileArc.UpdateArc(_speed, distance, Physics.gravity.magnitude, m_currentRadian, direction, true);
        SetLaunchPoint(direction, m_currentRadian * Mathf.Rad2Deg);
    }

    private void SetLaunchPoint(Vector3 planarDirection, float turretAngle)
    {
        launchPoint.rotation = Quaternion.AngleAxis(-turretAngle, transform.right)
            * Quaternion.LookRotation(planarDirection);
    }

    public void Launch()
    {
        print("Launch");
        Player player = GetComponent<Player>();

        //if(player.holdingItem == null)
        //{
        //    GameObject item = Instantiate(Resources.Load("PickableItem") as GameObject, launchCube.position, Quaternion.identity);
        //    player.holdingItem = item.transform;
        //}

        //if (player.holdingItem != null)
        //{
        //    // For debuging
        //    if (Time.time > lastShotTime + cooldown)
        //    {
        //        player.holdingItem.SetParent(null);
        //        player.holdingItem.GetComponent<Rigidbody>().isKinematic = false;
        //        player.holdingItem.GetComponent<Rigidbody>().velocity =
        //            launchPoint.forward * m_currentSpeed;

        //        Vector3 torq = Quaternion.Euler(0, 90, 0) * transform.rotation * launchPoint.localRotation * new Vector3(10, 0, 0);
        //        player.holdingItem.GetComponent<Rigidbody>().AddRelativeTorque(torq, ForceMode.VelocityChange);
        //        player.holdingItem = null;
        //        lastShotTime = Time.time;
        //    }
        //}

        // Launch player itself
        player.IsInAir = true;
        GetComponent<Rigidbody>().velocity =
                    launchPoint.forward * m_currentSpeed;
    }
}
