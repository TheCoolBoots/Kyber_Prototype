using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreInternalCollisions : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.bounds.Contains(GetComponent<Collider>().bounds.min) && 
            other.bounds.Contains(GetComponent<Collider>().bounds.max)) // + new Vector3(.01f, .01f, .01f)
        {
            Physics.IgnoreCollision(other, GetComponent<Collider>());
        }
    }
}
