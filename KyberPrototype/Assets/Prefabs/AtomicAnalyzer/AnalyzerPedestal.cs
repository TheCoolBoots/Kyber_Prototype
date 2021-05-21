using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Kyber
{
    public class AnalyzerPedestal : MonoBehaviour
    {
        public GameObject canisterShadowPrefab;
        public Transform snapPoint;

        [HideInInspector]
        public GameObject currentCanister;
        [HideInInspector]
        public bool pedestalOccupied = false;

        private GameObject canisterShadow;
        private void Start()
        {
            if (canisterShadowPrefab != null)
            {
                canisterShadow = Instantiate(canisterShadowPrefab, snapPoint);
                canisterShadow.SetActive(false);
            }
            else
            {
                Debug.LogError("Forgot to assign canisterShadow to something");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!pedestalOccupied)
            {
                canisterShadow.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!pedestalOccupied && IsValidCanister(other))
            {
                Interactable otherInteractable = other.GetComponent<Interactable>();

                if (otherInteractable.attachedToHand)
                {
                    canisterShadow.SetActive(true);
                }
                else
                {
                    EngageCanister(other);
                }
            }
            else if(pedestalOccupied && currentCanister.GetComponent<Interactable>().attachedToHand)
            {
                DisengageCanister(currentCanister);
            }

        }

        private void EngageCanister(Collider other)
        {
            canisterShadow.SetActive(false);
            other.gameObject.transform.position = snapPoint.position;
            other.gameObject.transform.rotation = snapPoint.rotation;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            pedestalOccupied = true;
            currentCanister = other.gameObject;
        }

        private void DisengageCanister(GameObject canister)
        {
            canisterShadow.SetActive(true);
            canister.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            pedestalOccupied = false;
        }

        private bool IsValidCanister(Collider other)
        {
            return (other.gameObject.GetComponent<Interactable>() != null && other.gameObject.GetComponent<CanisterContent>() != null);
        }

        public CanisterContentData GetCurrentCanisterData()
        {
            return pedestalOccupied ? currentCanister.GetComponent<CanisterContent>()._data : null;
        }

    }
}

