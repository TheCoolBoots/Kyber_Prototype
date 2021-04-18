using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundContainer : MonoBehaviour
{
    GameObject contentsPrefab;

    private void Start()
    {
        Instantiate(contentsPrefab, transform);
    }
}
