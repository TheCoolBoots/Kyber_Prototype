using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyber;

public class AnalyzerPedestal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Compound otherCompound = other.gameObject.GetComponent<Compound>();
        if (otherCompound != null)
            Debug.Log($"Other collider name: {other.gameObject.name}");
    }
}
