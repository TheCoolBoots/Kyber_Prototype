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
        public int gridPoint;
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
                    atomSpawn.setupAtomSpawner("O", parent, position);
                    atomSpawn.ActivateSpawner();
                    atomSpawn.transformAtom(scale, rotation);
                }
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Nitrogen"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("N", parent, position);
                    atomSpawn.ActivateSpawner();
                    atomSpawn.transformAtom(scale, rotation);

                }
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Hydrogen"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("H", parent, position);
                    atomSpawn.ActivateSpawner();
                    atomSpawn.transformAtom(scale, rotation);
                }
                if (snapPoint.getCurrentSnappedItem().GetComponent<Atom>().atomData.scientificName.Equals("Carbon"))
                {
                    GameObject spawn = Instantiate(atomSpawnerPrefab, parent);
                    atomSpawn = spawn.GetComponent<AtomSpawner>();
                    atomSpawn.setupAtomSpawner("C", parent, position);
                    atomSpawn.ActivateSpawner();
                    atomSpawn.transformAtom(scale, rotation);
                }
            }
        }

        public void removeAtom()
        {
            atomSpawn.OnDestroy();
        }
    }
}