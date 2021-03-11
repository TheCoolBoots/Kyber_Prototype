using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(Interactable))]

public class JoystickSpringReturn : MonoBehaviour
{
    
    public float P = 1f;
    public float D = 10f;
    public Transform forceLocation;
    public bool enable = true;
    private Vector3 targetPos;
    private Vector3 lastPos;
    private Vector3 thisPos;
    private Rigidbody thisRigidbody;
    private Interactable interactable;

    void Start()
    {
        targetPos = transform.position;
        lastPos = targetPos;
        thisRigidbody = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
    }

    void Update()
    {
        if (!interactable.attachedToHand && enable)
        {
            thisPos = transform.position;

            Vector3 change = thisPos - lastPos;

            Vector3 proportional = P * (targetPos - thisPos);
            Vector3 dampener = D * change;

            Vector3 force = proportional - dampener;

            thisRigidbody.AddForceAtPosition(force, forceLocation.position);

            lastPos = thisPos;
        }
        else
        {
            lastPos = transform.position;
        }
    }
}
