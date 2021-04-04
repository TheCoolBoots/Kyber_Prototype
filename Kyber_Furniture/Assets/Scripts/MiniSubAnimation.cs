using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class MiniSubAnimation : MonoBehaviour
{
    public Transform leftController;        //left controller target transform
    public Transform rightController;       //right controller target transform
    public Transform manipulatorTransform;  //arm target transform
    public Transform poleIK;
    public Transform poleDeploy;
    public GameObject hand;                 //manipulator hand that has IK script

    public Transform rigBase;
    public Transform rigHandle;
    public Transform leftEngineMount;
    public Transform rightEngineMount;
    public Transform leftEngineTarget;
    public Transform rightEngineTarget;
    public Transform leftProp;
    public Transform rightProp;
    public Transform cruiseProp1;
    public Transform cruiseProp2;
 
    public Vector3 grabOffset;
    public bool leftControllerEquipped;     //When the controllers are grabbed remember to set these to true
    public bool rightControllerEquipped;

    private Vector3 leftControllerStart;
    private Vector3 rightControllerStart;
    private Vector3 manipulatorStart;
    private Vector3 poleStart;
    public float armSpeed = 2f;            //How fast the controller target moves arm target
    public float armScale = 6f;            //How much the controller target moves the arm target

    public float engineRotSpeed = 2f;      //How fast the side engines rotate
    public float propSpeed = 100f;         //How fast the propellers rotate

    // Start is called before the first frame update
    void Start()
    {
        leftControllerStart = leftController.localPosition;
        rightControllerStart = rightController.localPosition;
        manipulatorStart = manipulatorTransform.localPosition;
        poleStart = poleIK.localPosition;
    }

    void FixedUpdate()
    {
        //I'm putting this in fixed update incase people decide to do physics stuff with the arm

        //Left arm copies left controller position when equipped and returns to starting position when unequipped
        if (leftControllerEquipped)
        {
            Vector3 controllerDiff = leftController.localPosition - leftControllerStart;
            Vector3 armLerp = Vector3.Lerp(manipulatorTransform.localPosition, manipulatorStart + controllerDiff * armScale + grabOffset, Time.fixedDeltaTime * armSpeed);
            manipulatorTransform.localPosition = armLerp;
            Vector3 poleLerp = Vector3.Lerp(poleIK.localPosition, poleDeploy.localPosition, Time.fixedDeltaTime * armSpeed*4f);
            poleIK.localPosition = poleLerp;
            hand.GetComponent<FastIKFabric>().SnapBackStrength = 0f;
        }
        else
        {
            //When controller isn't in use it moves back to starting position
            Vector3 armLerp = Vector3.Lerp(manipulatorTransform.localPosition, manipulatorStart, Time.fixedDeltaTime * armSpeed);
            Vector3 controllerLerp = Vector3.Lerp(leftController.localPosition, leftControllerStart, Time.fixedDeltaTime * armSpeed);
            manipulatorTransform.localPosition = armLerp;
            leftController.localPosition = controllerLerp;
            Vector3 poleLerp = Vector3.Lerp(poleIK.localPosition, poleStart, Time.fixedDeltaTime * armSpeed);
            poleIK.localPosition = poleLerp;
            hand.GetComponent<FastIKFabric>().SnapBackStrength = 1f;
        }
        
        if (rightControllerEquipped)
        {
            //Right hand doesn't do anything yet.
        }
        else
        {
            //When controller isn't in use it moves back to starting position
            //I don't have anything that resets the controller rotation but it would go here if needed
            Vector3 controllerLerp = Vector3.Lerp(rightController.localPosition, rightControllerStart, Time.fixedDeltaTime * armSpeed);
            rightController.localPosition = controllerLerp;
        }
    }

    void Update()
    {
        //Animations that won't affect physics stuff

        //Engine rotates along one axis to point to rig target---------------------------------

        //This probably isn't the most efficient way to rotate this but ay it works
        Vector3 leftEngineDirection = leftEngineTarget.position - leftEngineMount.position;
        Vector3 leftRelDir = rigBase.InverseTransformDirection(leftEngineDirection);
        float leftRot = Mathf.Atan2(-leftRelDir.y, leftRelDir.z)*Mathf.Rad2Deg;
        Quaternion leftSlerp = Quaternion.Slerp(leftEngineMount.localRotation, Quaternion.Euler(leftRot - 90f, 0f, 0f), engineRotSpeed * Time.deltaTime);
        leftEngineMount.localRotation = leftSlerp;

        Vector3 rightEngineDirection = rightEngineTarget.position - rightEngineMount.position;
        Vector3 rightRelDir = rigBase.InverseTransformDirection(rightEngineDirection);
        float rightRot = Mathf.Atan2(-rightRelDir.y, rightRelDir.z) * Mathf.Rad2Deg;
        Quaternion rightSlerp = Quaternion.Slerp(rightEngineMount.localRotation, Quaternion.Euler(rightRot - 90f, 0f, 0f), engineRotSpeed * Time.deltaTime);
        rightEngineMount.localRotation = rightSlerp;

        //Rotate propellers based on handle position
        float sqrHandleDist = Vector3.SqrMagnitude(rigHandle.localPosition);
        float handleRot = rigHandle.localEulerAngles.z;
        if (handleRot > 180f)
        {
            handleRot = 360f - handleRot;   //gotta do this or things will invert
        }
        float sideEngineThrottle = propSpeed * Mathf.Clamp(sqrHandleDist + handleRot / 5f, 0f, 4f);
        float cruiseEngineThrottle = propSpeed * Mathf.Clamp(rigHandle.localPosition.y, -2f, 2f);
        //print(rigHandle.localPosition);
        leftProp.RotateAround(leftProp.position, leftProp.up, sideEngineThrottle * Time.deltaTime);
        rightProp.RotateAround(rightProp.position, rightProp.up, sideEngineThrottle * Time.deltaTime);
        cruiseProp1.RotateAround(cruiseProp1.position, -cruiseProp1.up, cruiseEngineThrottle * Time.deltaTime);
        cruiseProp2.RotateAround(cruiseProp1.position, cruiseProp1.up, cruiseEngineThrottle * Time.deltaTime);

        /*
        In the town where I was born
        Lived a man who sailed to sea
        And he told us of his life
        In the land of submarines
        So we sailed up to the sun
        'Til we found a sea of green
        And we lived beneath the waves
        In our yellow submarine
        */
    }
}
