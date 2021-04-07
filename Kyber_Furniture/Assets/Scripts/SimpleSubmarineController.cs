using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SimpleSubmarineController : MonoBehaviour
{
    public SteamVR_Action_Vector2 joystickInput;
    public SteamVR_Action_Boolean ascend;
    public SteamVR_Action_Boolean descend;

    public float thrusterMagnitude = 12f;
    public float turnMagnitude = .3f;
    public float ascendDescendMag = 3f;

    public GameObject rightControllerTarget;

    private bool enabled = false;

    private Rigidbody thisrigidbody;

    private Vector3 originalPos;

    void Start()
    {
        thisrigidbody = GetComponent<Rigidbody>();
        originalPos = rightControllerTarget.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (enabled)
        {
            AddThrusterForce();

            AddVerticalForce();

            AddRotationalForce();
        }
    }

    private void AddThrusterForce()
    {
        // force is relative to magnitude of current controller position minus original controller position
        Vector3 direction = transform.TransformDirection(rightControllerTarget.transform.localPosition - originalPos);

        // makes movement relative to the direction the submarine is facing, then ensures that it is in the XZ plane
        thisrigidbody.AddForce(thrusterMagnitude * Vector3.ProjectOnPlane(direction, Vector3.up));
    }

    private void AddVerticalForce()
    {
        if (ascend.state)
        {
            thisrigidbody.AddForce(ascendDescendMag * new Vector3(0, 1, 0));
        }
        if (descend.state)
        {
            thisrigidbody.AddForce(ascendDescendMag * new Vector3(0, -1, 0));
        }
    }

    private void AddRotationalForce()
    {
        thisrigidbody.AddTorque(new Vector3(0, joystickInput.axis.x * turnMagnitude, 0));
    }

    public void EnableSubControl()
    {
        enabled = true;
    }

    public void DisableSubControl()
    {
        enabled = false;
    }
}
