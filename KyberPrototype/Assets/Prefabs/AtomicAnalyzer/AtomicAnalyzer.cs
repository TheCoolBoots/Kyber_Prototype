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
                currentCompoundData = analyzerPedestal.currentCanister.GetComponent<Compound>().compoundData;
                InitializeAtomSpawners();
                active = true;
            }
            else
            {
                Debug.LogError($"Analyzer Enabled: {analyzerEnabled}, Pedestal Occupied: {analyzerPedestal.pedestalOccupied}");
            }
        }

        private void InitializeAtomSpawners()
        {
            string[] atoms = currentCompoundData.atomComponents.Split(' ');
            for (int i = 0; i < atoms.Length; i++)
            {
                GameObject spawner = Instantiate(atomSpawnerPrefab);
                spawner.GetComponent<AtomSpawner>().elementCharacter = atoms[i];
                spawner.GetComponent<AtomSpawner>().parent = spawnPoint;
                spawner.GetComponent<AtomSpawner>().spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z + i * spawnerSpacing);
                spawner.GetComponent<AtomSpawner>().ActivateSpawner();
                currentAtoms.Add(spawner);
            }
        }


        public void ResetAnalyzer()
        {
            foreach (GameObject spawner in currentAtoms)
            {
                GameObject.Destroy(spawner);
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

