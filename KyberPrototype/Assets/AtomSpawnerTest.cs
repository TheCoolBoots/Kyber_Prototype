using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyber;

public class AtomSpawnerTest : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject atomSpawner;
    // Start is called before the first frame update
    void Start()
    {
        GameObject spawner = Instantiate(atomSpawner);
        spawner.GetComponent<AtomSpawner>().elementCharacter = "H";
        spawner.GetComponent<AtomSpawner>().parent = spawnPosition;
        spawner.GetComponent<AtomSpawner>().ActivateSpawner();
    }

}
