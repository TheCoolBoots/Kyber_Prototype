using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public class AtomicAnalyzer : MonoBehaviour
    {
        public GameObject canisterPedestal;
        public GameObject atomSpawnerPrefab;
        public Transform spawnPoint;

        public float spawnerSpacing = .3f;

        public bool active = false;
        public bool analyzerEnabled = false;

        private AnalyzerPedestal analyzerPedestal;
        private CompoundData currentCompoundData;
        private List<GameObject> currentAtoms;

        private void Start()
        {
            currentAtoms = new List<GameObject>(5);

            if (AnalyzerPedestalValid())
            {
                analyzerPedestal = canisterPedestal.GetComponent<AnalyzerPedestal>();
                analyzerEnabled = true;
            }
            else
            {
                analyzerEnabled = false;
            }
        }


        public void AnalyzeCompound()
        {
            if (analyzerEnabled && analyzerPedestal.pedestalOccupied)
            {
                // send data to AnalyzerScreen and display it
                ResetAnalyzer();

                // retrieve data out of canister on pedestal
                currentCompoundData = analyzerPedestal.currentCanister.GetComponent<Compound>().compoundData;

                // using compound data, create spawners of specified elements
                if (currentCompoundData != null)
                    InitializeAtomSpawners();

                // set active flag to true
                active = true;
            }
            else
            {
                Debug.LogError($"Analyzer Enabled: {analyzerEnabled}, Pedestal Occupied: {analyzerPedestal.pedestalOccupied}");
            }
        }

        private void InitializeAtomSpawners()
        {
            string[] atoms = currentCompoundData.atomComponents.Split(' '); // "H O" -> ["H", "O"]


            // for each element in the atomComponents list, create an atom spawner
            for (int i = 0; i < atoms.Length; i++)
            {
                GameObject spawner = Instantiate(atomSpawnerPrefab);
                spawner.GetComponent<AtomSpawner>().elementCharacter = atoms[i];
                spawner.GetComponent<AtomSpawner>().parent = spawnPoint;

                // space out each atom spawner with spawnerSpacing between them
                spawner.GetComponent<AtomSpawner>().spawnPosition = new Vector3(spawnPoint.localPosition.x, spawnPoint.localPosition.y, spawnPoint.localPosition.z - i * spawnerSpacing);
                spawner.GetComponent<AtomSpawner>().ActivateSpawner();
                currentAtoms.Add(spawner);
            }
        }

        // delete all active atom spawners and set active to false
        public void ResetAnalyzer()
        {
            foreach (GameObject spawner in currentAtoms)
            {
                Destroy(spawner);
            }
            currentAtoms = new List<GameObject>(5);

            active = false;
        }

        private bool AnalyzerPedestalValid()
        {
            if (canisterPedestal.GetComponent<AnalyzerPedestal>() == null)
            {
                Debug.LogError("Given pedestal game object does not contain AnalyzerPedestal script");
                return false;
            }
            return true;
        }
    }

}

