using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ActivateAtomSpawner : MonoBehaviour
{
    public UnityEvent activationEvents;
    // Start is called before the first frame update
    void Start()
    {
        activationEvents.Invoke();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
