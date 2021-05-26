using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float moveSpeed = 0.1f;
    //private float scrollSpeed = 10f;

    private float sensitivity = 5.0f;
    float lowerLimit = -20f;
    float upperLimit = 80f;
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Vector3 moveVector = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
            transform.position += moveSpeed * moveVector;
            //transform.position += moveSpeed * transform.forward * Input.GetAxisRaw("Horizontal");
            //transform.position += moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        /*
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
        }
        */

        float lookX = sensitivity * Input.GetAxis("Mouse X");
        float lookY = -sensitivity * Input.GetAxis("Mouse Y");
        transform.Rotate(lookY, lookX, 0.0f);

        float localX = transform.localEulerAngles.x;
        if (localX > 180f)
        {
            localX -= 360f;
        }

        if (localX < lowerLimit)
        {
            cameraStraight(lowerLimit);
        }
        else if (localX > upperLimit)
        {
            cameraStraight(upperLimit);
        }
        else
        {
            cameraStraight(transform.localEulerAngles.x);
        }
    }

    void cameraStraight(float localX)
    {
        transform.localEulerAngles = new Vector3(
                    localX,
                    transform.localEulerAngles.y,
                    0.0f);
    }
}
