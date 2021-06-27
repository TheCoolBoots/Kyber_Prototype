using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Kyber
{ 
    public class AtomSpawner : MonoBehaviour
    {

        [HideInInspector]
        public string elementCharacter;
        [HideInInspector]
        public Transform parent;
        [HideInInspector]
        public Vector3 spawnPosition;

        private GameObject atomModel;
        private GameObject currentAtom;
        private Interactable currentInteractable;
        private AtomData atomData;
        private CanisterContentData atomData1;

        public bool active = false;


        public void ActivateSpawner()
        {
            atomModel = Resources.Load($"Atom") as GameObject;
            if (!this.atomModel)
            {
                Debug.LogError($"Atom prefab not found in /Resources");
                return;
            }

            atomData = Resources.Load($"ElementData/{elementCharacter}") as AtomData;
            
            InstantiateNewAtom();
            currentAtom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            

            active = true;
        }

        private void Update()
        {
            if (active && currentInteractable.attachedToHand)
            {
                currentAtom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                InstantiateNewAtom();
                currentAtom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private void InstantiateNewAtom()
        {
            currentAtom = Instantiate(atomModel, parent.parent);
            currentInteractable = currentAtom.GetComponent<Interactable>();
            currentAtom.GetComponent<Atom>().atomData = atomData;
            currentAtom.GetComponent<Atom>().outerDiameter = .5f;
            currentAtom.GetComponent<Atom>().waitToLoad = false;
            currentAtom.transform.localPosition = spawnPosition;
        }

        public void OnDestroy()
        {
            Destroy(currentAtom);
        }
    }
}

