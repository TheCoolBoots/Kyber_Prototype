using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCam : MonoBehaviour
{
    public Camera useCamera;
    public Transform trackObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        useCamera.transform.LookAt(trackObject);
    }
}
