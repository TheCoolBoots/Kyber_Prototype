using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class atomInfoText : MonoBehaviour
{
    public TextMeshProUGUI atomName;
    public TextMeshProUGUI atomNumber;
    public TextMeshProUGUI atomMass;
    public TextMeshProUGUI atomSymbol;
    public TextMeshProUGUI atomValanceElce;
    public TextMeshProUGUI atomDensity;


    // Start is called before the first frame update
    void Start()
    {
        atomName.text = "testing";
        atomName.fontSize = 0.01f;
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
