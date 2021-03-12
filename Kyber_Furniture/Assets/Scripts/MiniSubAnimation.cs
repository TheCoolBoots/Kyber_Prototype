using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSubAnimation : MonoBehaviour
{
    public Transform leftController;
    public Transform manipulatorTransform;
    public Vector3 grabOffset;
    public bool leftControllerGrabbed;
    public bool rightControllerGrabbed;

    private Vector3 leftControllerStart;
    private Vector3 manipulatorStart;
    private float armSpeed = 2f;
    private float armScale = 6f;

    // Start is called before the first frame update
    void Start()
    {
        leftControllerStart = leftController.localPosition;
        manipulatorStart = manipulatorTransform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (leftControllerGrabbed)
        {

            Vector3 controllerDiff = leftController.localPosition - leftControllerStart;
            Vector3 armLerp = Vector3.Lerp(manipulatorTransform.localPosition, manipulatorStart + controllerDiff * armScale + grabOffset, Time.fixedDeltaTime * armSpeed);
            manipulatorTransform.localPosition = armLerp;
        }
        else
        {
            Vector3 armLerp = Vector3.Lerp(manipulatorTransform.localPosition, manipulatorStart, Time.fixedDeltaTime * armSpeed);
            Vector3 controllerLerp = Vector3.Lerp(leftController.localPosition, leftControllerStart, Time.fixedDeltaTime * armSpeed);
            manipulatorTransform.localPosition = armLerp;
            leftController.localPosition = controllerLerp;
        }
        
    }
}
