/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAtom : MonoBehaviour
{
    public static GenerateAtom Instance;
    public GameObject atom;

    void Start()
    {

    }

    public void generateAtom()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Instantiate(atom, new Vector3(-1, 1, 0), Quaternion.identity);
        }
        else if (this != Instance)
        {
            Debug.Log("Destroying extra GM");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateAtom : MonoBehaviour
{
    public GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void generateAtom()
    {
        Instantiate(myPrefab, new Vector3(-0.75f, 1, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
