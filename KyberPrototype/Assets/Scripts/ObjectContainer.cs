using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectContainer : MonoBehaviour
{
    public GameObject objectPrefab;
    public bool gravityAffectsObjects;
    public int numberOfInstances;

    public Mesh containerShape;

    [Header("Initial Velocity Bounds")]
    public Vector3 VelLowerBound;
    public Vector3 VelUpperBound;

    private List<GameObject> objects;
    private Mesh invertedContainerMesh;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InvertContainerMesh()
    {
        invertedContainerMesh = containerShape;
        invertedContainerMesh.triangles = invertedContainerMesh.triangles.Reverse().ToArray();   
    }
}
