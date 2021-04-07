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

    public float thrusterMagnitude = 3f;
    public float turnMagnitude = .3f;
    public float ascendDescendMag = 3f;

    private bool enabled = false;

    private Rigidbody thisrigidbody;

    void Start()
    {
        thisrigidbody = GetComponent<Rigidbody>();
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
        Vector3 direction = transform.TransformDirection(new Vector3(0, -joystickInput.axis.y, 0));

        // makes movement relative to the direction the player is facing, then ensures that it is in the XZ plane
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
        // transform.Rotate(new Vector3(0, 0, joystickInput.axis.x * turnMagnitude));
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
