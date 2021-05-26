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

        public bool active = false;


        public void ActivateSpawner()
        {
            this.atomModel = Resources.Load($"AtomPlaceholders/{ elementCharacter }atom") as GameObject;
            if (!this.atomModel)
            {
                Debug.LogError($"Atom Placeholder AtomPlaceholders/{ elementCharacter }atom not found");
            }

            currentAtom = Instantiate(atomModel, parent.parent);
            currentInteractable = currentAtom.GetComponent<Interactable>();
            currentAtom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentAtom.transform.localPosition = spawnPosition;

            active = true;
        }

        private void Update()
        {
            if (active && currentInteractable.attachedToHand)
            {
                currentAtom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                currentAtom = Instantiate(atomModel, parent);
                currentInteractable = currentAtom.GetComponent<Interactable>();
                currentAtom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                currentAtom.transform.position = spawnPosition;
            }
        }

        public void OnDestroy()
        {
            Destroy(currentAtom);
        }
    }
}

