using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kyber
{
    public class Compound : MonoBehaviour
    {
        public CompoundIDs compound = CompoundIDs.PureWater;
        public TextMesh compoundLabel;

        [HideInInspector]
        public CompoundData compoundData;

        private GameObject compoundModel;
        private Dictionary<CompoundIDs, CompoundData> compoundTableRef;
        

        private void Start()
        {

            // need to research if this method of getting a reference to a dictionary is efficient or not
            compoundTableRef = GameObject.Find("CompoundDatabase").GetComponent<CompoundTable>().compoundTable;

            if (compoundTableRef == null)
            {
                Debug.LogError("ERROR: Create a game object with the name CompoundDatabase and add the CompoundTable script to it");
                return;
            }

            compoundTableRef.TryGetValue(compound, out compoundData);
            if(compoundData == null)
            {
                Debug.LogError($"ERROR: Compound ID:{ compound } not found in Compound Table");
                return;
            }

            GameObject prefab = Resources.Load(compoundData.resourceFilepath) as GameObject;
            if(prefab == null)
            {
                Debug.LogError($"ERROR: Prefab ID:{ compound } not found at given resourceFilepath");
                return;
            }

            compoundLabel.text = $"{compoundData.commonName}: {compoundData.state}";
            compoundModel = Instantiate(Resources.Load(compoundData.resourceFilepath) as GameObject, transform);
            // Debug.Log($"Resource Filepath: {compoundData.resourceFilepath}\tPrefab Name: {compoundModel.name}");
            compoundModel.transform.localScale = new Vector3(compoundData.modelScale, compoundData.modelScale, compoundData.modelScale);
            



            // LATER ON:
            // check if stable
            // decay if need be
            // add effects accordingly
        }

    }
}

