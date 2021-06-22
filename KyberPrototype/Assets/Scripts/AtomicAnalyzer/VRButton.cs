using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class VRButton : MonoBehaviour
{
    public float travelDistance = .2f;
    public bool activated = false;
    public float activationThreshold = .01f;
    public float returnSpeed = .1f;

    public UnityEvent buttonClickedEvent;

    private Vector3 startPos;
    public bool beingPressed;

    private bool lastState = false;

    void Start()
    {
        startPos = transform.position;
        buttonClickedEvent.AddListener(ping);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(startPos.x, transform.position.y, startPos.z);

        if (!beingPressed)
        {
            newPos.y += returnSpeed * Time.deltaTime;
        }

        if (newPos.y < startPos.y - travelDistance)
        {
            newPos.y = startPos.y - travelDistance;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (newPos.y > startPos.y)
        {
            newPos.y = startPos.y;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        transform.position = newPos;

        if(transform.position.y > startPos.y - travelDistance - activationThreshold && 
            transform.position.y < startPos.y - travelDistance + activationThreshold)
            activated = true;
        else
            activated = false;

        if(lastState == false && activated == true)
        {
            buttonClickedEvent.Invoke();
        }
        lastState = activated;

    }

    private void OnCollisionEnter(Collision collision)
    {
        beingPressed = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        beingPressed = false;
    }

    private void ping()
    {
        Debug.Log("Event pinged");
    }
}
