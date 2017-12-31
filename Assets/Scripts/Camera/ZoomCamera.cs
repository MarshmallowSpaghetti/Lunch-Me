using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ZoomCamera : MonoBehaviour
{
    [SerializeField]
    private float incline = 45;

    [SerializeField]
    private float minDistance = 10;

    [SerializeField]
    private float maxDistance = 20;

    private float currentDistance;

    [SerializeField]
    private float zoomSensitivity = 1;

    private float zoomLevel;

    public Transform target;
    private Camera m_camera;

    public Camera ThisCamera
    {
        get
        {
            if (m_camera == null)
                m_camera = GetComponent<Camera>();
            return m_camera;
        }
    }

    void Awake()
    {
        transform.rotation = Quaternion.AngleAxis(incline, Vector3.right);
        zoomLevel = 0.5f;
        currentDistance = Mathf.Lerp(maxDistance, minDistance, zoomLevel);
        transform.position = target.position - transform.forward * currentDistance;
    }

    void Update()
    {
        float previousZoomLevel = zoomLevel;
        float zoomInput = -Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        //print("input " + zoomInput);
        if (zoomInput.Sgn() > 0)
            zoomLevel = Mathf.Lerp(zoomLevel, 1, Mathf.Abs(zoomInput) * Time.deltaTime);
        else if (zoomInput.Sgn() < 0)
            zoomLevel = Mathf.Lerp(zoomLevel, 0, Mathf.Abs(zoomInput) * Time.deltaTime);
        
        Vector3 projectVec = Vector3.Dot(transform.forward, target.position - transform.position) * transform.forward;
        Vector3 projectPos = transform.position + projectVec;
        
        transform.position = projectPos -
            transform.forward * (zoomLevel * (maxDistance - minDistance) + minDistance);
    }
}
