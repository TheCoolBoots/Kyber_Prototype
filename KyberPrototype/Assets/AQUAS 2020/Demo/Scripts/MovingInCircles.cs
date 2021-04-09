using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingInCircles : MonoBehaviour {

    public GameObject pivot;
    public float speed = 40;
    public bool clockwise;
    public bool oscillate;
    float t = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame.
	void Update () {

        t += Time.deltaTime;

        if (oscillate)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+ Mathf.Sin(t) * 0.015f, transform.position.z);
        }

        if (pivot == null)
        {
            return;
        }

        if (clockwise)
        {
            transform.RotateAround(pivot.transform.position, Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(pivot.transform.position, Vector3.up, -speed * Time.deltaTime);
        }

        

    }
}
