using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondIndicator : MonoBehaviour
{
    [SerializeField] private MeshRenderer leftBond;
    [SerializeField] private MeshRenderer rightBond;
    [SerializeField] private MeshRenderer centerBond;

    public int numBonds = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (numBonds)
        {
            case 1:
                leftBond.enabled = false;
                rightBond.enabled = false;
                centerBond.enabled = true;
                break;
            case 2:
                leftBond.enabled = true;
                rightBond.enabled = true;
                centerBond.enabled = false;
                break;
            case 3:
                leftBond.enabled = true;
                rightBond.enabled = true;
                centerBond.enabled = true;
                break;
            default:
                leftBond.enabled = false;
                rightBond.enabled = false;
                centerBond.enabled = false;
                break;
        }
    }
}
