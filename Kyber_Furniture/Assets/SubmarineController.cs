
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(CharacterJoint))]
[RequireComponent(typeof(Interactable))]

public class SubmarineController : MonoBehaviour
{
    public GameObject submarine;
    public GameObject teleporting;

    public SteamVR_Action_Single speedControl;
    public SteamVR_Action_Boolean altitudeControl;
    public float thrusterPower = 10;
    public float turningPower = 10;
    public float elevationPower = 10;

    private Interactable interactable;
    private CharacterJoint joystickJoint;

    private float zRotationMax;
    private float zRotationMin;
    private float xRotationMax;
    private float xRotationMin;

    private int count = 0;

    void Start()
    {
        joystickJoint = GetComponent<CharacterJoint>();
        interactable = GetComponent<Interactable>();

        GetJoystickRotationLimits();
    }

    private void GetJoystickRotationLimits()
    {
        xRotationMax = joystickJoint.highTwistLimit.limit;
        xRotationMin = joystickJoint.lowTwistLimit.limit;
        zRotationMax = joystickJoint.swing2Limit.limit;
        zRotationMin = -zRotationMax;

        Debug.Log($"xMax: {xRotationMax}\txMin: {xRotationMin}\tzMax: {zRotationMax}\tzMin: {zRotationMin}");
    }

    // Update is called once per frame
    void Update()
    {
                if(count == 100)
                {
                    Debug.Log($"transformZ: {transform.localRotation.eulerAngles.z}\ttransformX: {transform.localRotation.eulerAngles.x}");
                    count = 0;
                }
                count++;


        //if (interactable.attachedToHand)
        //{
         //   CalculateMovementForces(out float forwardForce, out float verticalForce, out float turningForce);
        //    ApplyForcesToSubmarine(forwardForce, verticalForce, turningForce);
        //}
    }

    private void CalculateMovementForces(out float forwardForce, out float verticalForce, out float turningForce)
    {
        forwardForce = speedControl.axis * thrusterPower;
        verticalForce = transform.localRotation.z * turningPower * 180 / zRotationMax;
        turningForce = transform.localRotation.x * elevationPower * 180 / xRotationMax;
        Debug.Log($"forward force: {forwardForce} \taltitude power: {turningForce} \tyaw power: {verticalForce}");
    }

    private void ApplyForcesToSubmarine(float forwardForce, float verticalForce, float turningForce)
    {
        Vector3 direction = submarine.transform.TransformDirection(Vector3.forward);

        submarine.GetComponent<Rigidbody>().AddForce(direction * forwardForce);

        submarine.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, turningForce, 0));

        if (altitudeControl.state)
        {
            submarine.GetComponent<Rigidbody>().AddForce(Vector3.up * verticalForce);
        }
    }

    private void OnHandHoverBegin()
    {
        teleporting.SetActive(false);
    }
    private void OnHandHoverEnd()
    {
        teleporting.SetActive(true);
    }
}
