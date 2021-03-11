
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(CharacterJoint))]
[RequireComponent(typeof(Interactable))]

public class SubmarineController : MonoBehaviour
{
    public GameObject submarine;

    public SteamVR_Action_Single speedControl;
    public float thrusterForce = 10;
    public float turningForce = 10;
    public float breakingForce = 10;

    private Interactable interactable;
    private CharacterJoint joystickJoint;


    // rotation around the z axis controls motion in the x axis
    // rotation around the x axis controls motion in the z axis
    // positive x axis is forward

    private float zRotationMax;
    private float zRotationMin;
    private float xRotationMax;
    private float xRotationMin;

    void Start()
    {
        joystickJoint = GetComponent<CharacterJoint>();
        interactable = GetComponent<Interactable>();

        xRotationMax = joystickJoint.lowTwistLimit.limit;
        xRotationMin = joystickJoint.highTwistLimit.limit;
        zRotationMax = joystickJoint.swing2Limit.limit;
        zRotationMin = -zRotationMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.attachedToHand)
        {
            float forwardForce = speedControl.axis * thrusterForce;
            float pitchPower = - transform.rotation.z * turningForce / zRotationMax;
            float yawPower = - transform.rotation.x * turningForce / xRotationMax;
            // Debug.Log($"forward force: {forwardForce} \tpitch power: {pitchPower} \tyaw power: {yawPower}");

            submarine.GetComponent<Rigidbody>().AddForce(Vector3.forward * forwardForce);

        }
    }
}
