using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MouseInput : BaseSingletonMono<MouseInput>
{
    private Vector3 m_mousePos;

    public Vector3 MousePos
    {
        get
        {
            return m_mousePos;
        }

        private set
        {
            m_mousePos = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseHitPos();
    }

    private void UpdateMouseHitPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(CrossPlatformInputManager.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground")))
        {
            MousePos = hit.point;
        }
    }
}
