using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Kyber
{
    public class AtomSpawner : MonoBehaviour
    {
        public string elementCharacter;
        public Transform parent;
        public Vector3 spawnPosition;

        private GameObject atomModel;
        private GameObject currentAtom;
        private Interactable currentInteractable;
        private AtomData atomData;

        public bool active = false;

        public void setupAtomSpawner(string character, Transform p, Vector3 position)
        {
            elementCharacter = character;
            parent = p;
            spawnPosition = position;
        }

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
            currentAtom.transform.Rotate(0, 0, 90, Space.Self);
        }

        public void transformAtom(Vector3 scale, Vector3 rotation)
        {
            currentAtom.transform.localScale = scale;
            currentAtom.transform.Rotate(rotation);
        }


        public void OnDestroy()
        {
            Destroy(currentAtom);
        }
    }
}

