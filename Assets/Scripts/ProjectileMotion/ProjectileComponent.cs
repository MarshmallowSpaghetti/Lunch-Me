using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    Transform fireBase;
    
    public ProjectileArc projectileArc;

    private float currentSpeed;
    private float currentRadian;

    private Vector3 targetPoint;

    [SerializeField]
    float cooldown = 0.1f;
    private float lastShotTime;

    private void Start()
    {
        //Physics.gravity = new Vector3(0f, -50f, 0f);
        //print("gravity " + Physics.gravity);
    }

    public void SetEnable(bool _isEnable)
    {
        projectileArc.gameObject.SetActive(_isEnable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPoint, 0.2f);
    }

    public void SetTargetWithAngle(Vector3 _target, float _angleInDeg)
    {
        currentRadian = _angleInDeg * Mathf.Deg2Rad;

        targetPoint = _target;
        //GizmosHelper.DrawBox(point, Vector3.one * 0.2f, Color.yellow);
        Vector3 direction = _target - firePoint.position;

        float yOffset = direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        currentSpeed = ProjectileMath.CalculateLaunchSpeed(distance, yOffset, Physics.gravity.magnitude, _angleInDeg * Mathf.Deg2Rad);

        projectileArc.UpdateArc(currentSpeed, distance, Physics.gravity.magnitude, currentRadian, direction, true);
        SetThrowPoint(direction, currentRadian * Mathf.Rad2Deg);
    }

    public float GetRequiredSpeed(Vector3 point, float angle)
    {
        Vector3 direction = point - firePoint.position;
        
        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = direction.magnitude;

        return ProjectileMath.CalculateLaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);
    }

    public void SetTargetWithSpeed(Vector3 _target, float _speed, bool _useLowAngle)
    {
        currentSpeed = _speed;

        Vector3 direction = _target - firePoint.position;
        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = direction.magnitude;

        float angle0, angle1;
        bool targetInRange = ProjectileMath.CalculateLaunchAngle(_speed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);

        if (targetInRange)
            currentRadian = _useLowAngle ? angle1 : angle0;

        projectileArc.UpdateArc(_speed, distance, Physics.gravity.magnitude, currentRadian, direction, targetInRange);
        SetThrowPoint(direction, currentRadian * Mathf.Rad2Deg);
    }

    public void SetTargetWithBothAngleAndSpeed(Vector3 _target, float _angleInDeg, float _speed)
    {
        currentRadian = _angleInDeg * Mathf.Deg2Rad;
        currentSpeed = _speed;

        Vector3 direction = _target - firePoint.position;
        float yOffset = direction.y;
        //direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        direction = direction.SetY(0);// Equal to the line above
        float distance = ProjectileMath.CalculateLandDistance(firePoint.position.y, Physics.gravity.magnitude, _angleInDeg, _speed);

        projectileArc.UpdateArc(_speed, distance, Physics.gravity.magnitude, currentRadian, direction, true);
        SetThrowPoint(direction, currentRadian * Mathf.Rad2Deg);
    }

    public void Launch()
    {
        print("fire");
        Player player = GetComponent<Player>();

        //if(player.holdingItem == null)
        //{
        //    GameObject item = Instantiate(Resources.Load("PickableItem") as GameObject, firePoint.position, Quaternion.identity);
        //    player.holdingItem = item.transform;
        //}

        if (player.holdingItem != null)
        {
            // For debuging
            if (Time.time > lastShotTime + cooldown)
            {
                player.holdingItem.SetParent(null);
                player.holdingItem.GetComponent<Rigidbody>().isKinematic = false;
                player.holdingItem.GetComponent<Rigidbody>().velocity =
                    firePoint.up * currentSpeed;

                Vector3 torq = Quaternion.Euler(0, 90, 0) * transform.rotation * fireBase.localRotation * new Vector3(10, 0, 0);
                player.holdingItem.GetComponent<Rigidbody>().AddRelativeTorque(torq, ForceMode.VelocityChange);
                player.holdingItem = null;
                lastShotTime = Time.time;
            }
        }
    }

    private void SetThrowPoint(Vector3 planarDirection, float turretAngle)
    {
        fireBase.rotation = Quaternion.LookRotation(planarDirection) * Quaternion.Euler(-90, -90, 0);
        firePoint.localRotation = Quaternion.Euler(90, 90, 0) * Quaternion.AngleAxis(turretAngle, Vector3.forward);
    }
}
