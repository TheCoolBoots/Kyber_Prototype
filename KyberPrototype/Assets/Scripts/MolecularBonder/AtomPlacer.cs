using UnityEngine;

public class AtomPlacer : MonoBehaviour
{
    public int gridPoint;
    public GameObject atom;
    private GameObject atom1;
    private GameObject atom2;
    private GameObject atom3;
    private GameObject atom4;
    private GameObject atom5;
    private GameObject atom6;
    private GameObject atom7;
    private GameObject atom8;
    private GameObject atom9;

    void Start()
    {

    }

    private void Update()
    {

    }

    public void placeAtom()
    {
        if (gridPoint == 1)
        {
            atom1 = Instantiate(atom, new Vector3(-1, 1, 4.8f), Quaternion.identity);
            atom1.transform.localScale = new Vector3(3, 3, 3);
            atom1.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 2)
        {
            atom2 = Instantiate(atom, new Vector3(-1, 2, 4.8f), Quaternion.identity);
            atom2.transform.localScale = new Vector3(3, 3, 3);
            atom2.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 3)
        {
            atom3 = Instantiate(atom, new Vector3(-1, 3, 4.8f), Quaternion.identity);
            atom3.transform.localScale = new Vector3(3, 3, 3);
            atom3.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 4)
        {
            atom4 = Instantiate(atom, new Vector3(0, 1, 4.8f), Quaternion.identity);
            atom4.transform.localScale = new Vector3(3, 3, 3);
            atom4.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 5)
        {
            atom5 = Instantiate(atom, new Vector3(0, 2, 4.8f), Quaternion.identity);
            atom5.transform.localScale = new Vector3(3, 3, 3);
            atom5.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 6)
        {
            atom6 = Instantiate(atom, new Vector3(0, 3, 4.8f), Quaternion.identity);
            atom6.transform.localScale = new Vector3(3, 3, 3);
            atom6.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 7)
        {
            atom7 = Instantiate(atom, new Vector3(1, 1, 4.8f), Quaternion.identity);
            atom7.transform.localScale = new Vector3(3, 3, 3);
            atom7.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 8)
        {
            atom8 = Instantiate(atom, new Vector3(1, 2, 4.8f), Quaternion.identity);
            atom8.transform.localScale = new Vector3(3, 3, 3);
            atom8.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        if (gridPoint == 9)
        {
            atom9 = Instantiate(atom, new Vector3(1, 3, 4.8f), Quaternion.identity);
            atom9.transform.localScale = new Vector3(3, 3, 3);
            atom9.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
    }

    public void removeAtom()
    {
        if (gridPoint == 1)
        {
            Destroy(atom1);
        }
        if (gridPoint == 2)
        {
            Destroy(atom2);
        }
        if (gridPoint == 3)
        {
            Destroy(atom3);
        }
        if (gridPoint == 4)
        {
            Destroy(atom4);
        }
        if (gridPoint == 5)
        {
            Destroy(atom5);
        }
        if (gridPoint == 6)
        {
            Destroy(atom6);
        }
        if (gridPoint == 7)
        {
            Destroy(atom7);
        }
        if (gridPoint == 8)
        {
            Destroy(atom8);
        }
        if (gridPoint == 9)
        {
            Destroy(atom9);
        }
    }
}