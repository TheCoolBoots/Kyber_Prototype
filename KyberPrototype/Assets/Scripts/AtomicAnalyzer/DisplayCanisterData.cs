using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayCanisterData : MonoBehaviour
{
    [SerializeField] private AtomicAnalyzer atomicAnalyzer;

    [SerializeField] private TextMeshPro textLabel;


    // Update is called once per frame
    public void UpdateCanisterDisplay()
    {
        textLabel.text = atomicAnalyzer.currentCompoundData.commonName;
    }
}
