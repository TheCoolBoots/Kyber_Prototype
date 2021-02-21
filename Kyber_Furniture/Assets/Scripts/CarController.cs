using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CarController : MonoBehaviour
{
    // get all wheel colliders
    // get all visual wheels
    // get controller input
    // apply controller input to colliders
    // match visual wheel transforms to wheel colliders

    [System.Serializable]
    public struct WheelColliderPair
    {
        public GameObject visualWheel;
        public WheelCollider wheelCollider;
    }

    public SteamVR_Action_Vector2 input;

    public List<WheelColliderPair> wheelColliderPairs;

    public float accelerationMultiplier = 800;
    public float maxTurnAngle = 45;

    public void FixedUpdate()
    {
        //float turnInput = Input.GetAxis("Horizontal");
        //float accelInput = Input.GetAxis("Vertical");

        float turnInput = input.axis.x;
        float accelInput = input.axis.y;

        MoveCar(turnInput, accelInput);
    }

    private void MoveCar(float turnInput, float accelInput)
    {
        foreach (WheelColliderPair pair in wheelColliderPairs)
        {

            if (pair.wheelCollider == null || pair.visualWheel == null)
            {
                Debug.LogError("Err: wheelColliderPair not instantiated");
            }
            else
            {
                // if the wheel is in the front half of the car
                if (pair.visualWheel.transform.localPosition.z > 0)
                {
                    pair.wheelCollider.steerAngle = turnInput * maxTurnAngle;
                }
                // if the wheel is in the rear half of the car
                else
                {
                    pair.wheelCollider.motorTorque = accelInput * accelerationMultiplier;
                }

                // ensure the wheel model follows the wheel collider
                pair.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                pair.visualWheel.transform.position = pos;
                pair.visualWheel.transform.rotation = rot;

            }

        }
    }
}
