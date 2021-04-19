using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public class Compound : MonoBehaviour
    {
        public float modelScale = .95f;
        public GameObject compoundTableReference;
        public CompoundIDs compound = CompoundIDs.PureWater;

        [HideInInspector]
        public CompoundData compoundData;

        private GameObject compoundModel;
        private Dictionary<CompoundIDs, CompoundData> compoundTableRef;
        

        private void Start()
        {
            // need to research if this method of getting a reference to a dictionary is efficient or not
            compoundTableRef = compoundTableReference.GetComponent<CompoundTable>().compoundTable;

            if(compoundTableRef == null)
            {
                Debug.LogError("ERROR: CompoundTable not found in given Table Reference");
                return;
            }
            compoundTableRef.TryGetValue(compound, out CompoundData tmp);
            if(tmp == null)
            {
                Debug.LogError($"ERROR: Compound ID:{ compound } not found in Compound Table");
                return;
            }
            compoundData = tmp;

            GameObject prefab = Resources.Load(compoundData.resourceFilepath) as GameObject;
            if(prefab == null)
            {
                Debug.LogError($"ERROR: Prefab ID:{ compound } not found at given resourceFilepath");
                return;
            }
            compoundModel = Instantiate(Resources.Load(compoundData.resourceFilepath) as GameObject, transform);
            compoundModel.transform.localScale = new Vector3(modelScale, modelScale, modelScale);



            // LATER ON:
            // check if stable
            // decay if need be
            // add effects accordingly
        }

    }
}

