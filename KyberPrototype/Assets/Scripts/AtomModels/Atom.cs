using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Kyber
{
    public class Atom : MonoBehaviour
    {
        private enum displayModes
        {
            Simple,
            BohrModel,
            ElectronClouds,
            OrbitalElectronsInClouds
        }

        public AtomData atomData;
        //public CanisterContentData atomData;
        [SerializeField] private GameObject bhorModelRingPrefab;
        [SerializeField] private GameObject electronPrefab;
        [SerializeField] private GameObject nucleusPrefab;

        [Space]
        public float outerDiameter;
        [SerializeField] private float electronSpeed;
        [SerializeField] private float electronScale;

        public ElectronOrbitalData electronOrbitalData;
        private displayModes currentDisplayMode = displayModes.BohrModel;

        private List<GameObject> bhorModelRings;
        private GameObject nucleus;
        private SphereCollider sphereCollider;

        public bool waitToLoad = false;


        void Start()
        {
            if(atomData == null)
            {
                waitToLoad = true;
            }
        }

        private void Update()
        {
            if (!waitToLoad)
            {
                LoadAtom();
                waitToLoad = true;
            }
        }

        public void LoadAtom()
        {
            sphereCollider = GetComponent<SphereCollider>();

            // NOTE: need to add feedback for when hovering over atom with hand
            sphereCollider.radius = outerDiameter / 14;

            // # electrons = # protons - charge
            electronOrbitalData = new ElectronOrbitalData(atomData.atomicNumber - atomData.charge);

            LoadNucleus();

            // NOTE: will have switch statement here to load one of many display modes
            LoadBhorModel();
        }

        private void LoadNucleus()
        {
            nucleus = Instantiate(nucleusPrefab, transform);
            AtomNucleus reference = nucleus.GetComponent<AtomNucleus>();

            // pass in all relevant data before initializing the proton/neutron prefabs
            reference.atomicNumber = atomData.atomicNumber;
            reference.massNumber = atomData.massNumber;

            // make the radius an arbitrary 20th the size of the whole model
            reference.nucleusDiameter = outerDiameter/30;

            reference.InitializeAtomNucleus();
        }

        private void LoadBhorModel()
        {
            bhorModelRings = new List<GameObject>(); // reset the list of bhor model rings

            int[] bhorElectronEnergyLevels = electronOrbitalData.GetBhorModelData();   // grab an array with # of electrons at each energy level (1-7, no orbitals)

            for(int i = 0; i < bhorElectronEnergyLevels.Length; i++)
            {
                if(bhorElectronEnergyLevels[i] > 0)
                {
                    GameObject thisRing = Instantiate(bhorModelRingPrefab, transform);
                    BhorModelRing reference = thisRing.GetComponent<BhorModelRing>();

                    // pass in all relevant data before initializing the ring @ energy level i + 1
                    reference.electronPrefab = electronPrefab;
                    reference.numElectrons = bhorElectronEnergyLevels[i];
                    reference.ringRadius = (i + 1) * outerDiameter / 14;    // 7 energy levels so radius should be level * radius / 7
                    reference.electronSpeed = electronSpeed;
                    reference.electronScale = electronScale;
                    reference.InitializeElectrons();
                    bhorModelRings.Add(thisRing);
                }
            }
        }


        // the first time an atom is picked up, set all the protons/neutrons to kinematic so they keep in place
        private void OnAttachedToHand()
        {
            nucleus.GetComponent<AtomNucleus>().SetAllToKinematic(true);
        }
    }
}

