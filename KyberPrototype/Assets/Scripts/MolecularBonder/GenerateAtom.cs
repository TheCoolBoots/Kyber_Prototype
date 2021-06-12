using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateAtom : MonoBehaviour
{
    public GameObject myPrefab;
    public float x, y, z;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
        }
    }

    public void generateAtom()
    {
        Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
