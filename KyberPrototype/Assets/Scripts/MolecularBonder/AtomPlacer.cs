using UnityEngine;
using Kyber;

namespace Kyber
{
    public class AtomPlacer : MonoBehaviour
    {
        private GameObject atom;
        public Transform parent;
        [SerializeField] private VRSnapPoint snapPoint;
        [SerializeField] private AtomSpawner atomSpawn;
        [SerializeField] private GameObject atomSpawnerPrefab;
/*        private GameObject atom1;
        private GameObject atom2;
        private GameObject atom3;
        private GameObject atom4;
        private GameObject atom5;
*/        public int gridPoint;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 rotation;

        public void placeAtom()
        {
            if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>() != null)
            {
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Oxygen"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("0", parent, position);
                    atomSpawn.ActivateSpawner();
                }
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Nitrogen"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("N", parent, position);
                    atomSpawn.ActivateSpawner();
                }
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Hydrogen"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("H", parent, position);
                    atomSpawn.ActivateSpawner();
                }
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Carbon"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("C", parent, position);
                    atomSpawn.ActivateSpawner();
                }
            }
        }

        private void transformAtom(GameObject atom)
        {
            atom.transform.localScale = scale;
            atom.transform.Rotate(rotation);
        }
        public void removeAtom()
        {
/*            if (gridPoint == 1)
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
*/        }
    }
}