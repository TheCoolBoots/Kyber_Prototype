using UnityEngine;

public class AtomPlacer : MonoBehaviour
{
    public int gridPoint;
    public GameObject atom;

    void Start()
    {
        placeAtom();
    }

    private void Update()
    {

    }

    public void placeAtom()
    {
        if (gridPoint == 1)
        {
            Instantiate(atom, new Vector3(-1, 0, 3), Quaternion.identity);
        }
        if (gridPoint == 2)
        {
            Instantiate(atom, new Vector3(-1, 1, 3), Quaternion.identity);
        }
        if (gridPoint == 3)
        {
            Instantiate(atom, new Vector3(-1, 2, 3), Quaternion.identity);
        }
        if (gridPoint == 4)
        {
            Instantiate(atom, new Vector3(0, 0, 3), Quaternion.identity);
        }
        if (gridPoint == 5)
        {
            Instantiate(atom, new Vector3(0, 1, 3), Quaternion.identity);
        }
        if (gridPoint == 6)
        {
            Instantiate(atom, new Vector3(0, 2, 3), Quaternion.identity);
        }
        if (gridPoint == 7)
        {
            Instantiate(atom, new Vector3(1, 0, 3), Quaternion.identity);
        }
        if (gridPoint == 8)
        {
            Instantiate(atom, new Vector3(1, 1, 3), Quaternion.identity);
        }
        if (gridPoint == 9)
        {
            Instantiate(atom, new Vector3(1, 2, 3), Quaternion.identity);
        }
    }
}