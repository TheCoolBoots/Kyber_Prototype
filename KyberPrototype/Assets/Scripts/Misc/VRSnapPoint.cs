using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

namespace Kyber
{
    public class VRSnapPoint : MonoBehaviour
    {
        [SerializeField] private Transform snapPoint;
        [SerializeField] private MeshRenderer colliderBoundsIndicator;
        [SerializeField] private Collider activationThreshold;

        [Space]
        [SerializeField] private UnityEvent engageEvents;
        [SerializeField] private UnityEvent disengageEvents;

        [Space]
        public bool snapPointOccipied = false;
        public GameObject currentSnappedItem;

        public GameObject getCurrentSnappedItem()
        {
            return currentSnappedItem;
        }

        private void Awake()
        {
            if (snapPoint == null)
                snapPoint = transform;
            if (colliderBoundsIndicator == null)
                colliderBoundsIndicator = GetComponent<MeshRenderer>();
            if (activationThreshold == null)
                activationThreshold = GetComponent<Collider>();

            activationThreshold.isTrigger = true;
            colliderBoundsIndicator.enabled = false;
        }


        private void OnTriggerExit(Collider other)
        {
            colliderBoundsIndicator.enabled = false;
        }

        private void OnTriggerStay(Collider other)
        {
            // if snap point is empty, and component colliding with snap point has a Interactable && Throwable component
            if (!snapPointOccipied && other.GetComponent<Interactable>() != null && other.GetComponent<Throwable>() != null)
            {
                if (!colliderBoundsIndicator.enabled)
                {
                    colliderBoundsIndicator.enabled = true;
                }

                Interactable otherInteractable = other.GetComponent<Interactable>();

                // engage the item to the snap point if it is within the collider bounds and not being grabbed
                if (!otherInteractable.attachedToHand)
                { 
                    EngageItem(other);
                }
            }

            // disengage the item if there is an item engaged and item is being picked up
            else if(snapPointOccipied && currentSnappedItem.GetComponent<Interactable>().attachedToHand)
            {
                DisengageItem(currentSnappedItem);
            }

        }


        private void EngageItem(Collider other)
        {
            // Debug.Log("Engaging item");
            other.gameObject.transform.position = snapPoint.position;
            other.gameObject.transform.rotation = snapPoint.rotation;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            snapPointOccipied = true;
            currentSnappedItem = other.gameObject;
            colliderBoundsIndicator.enabled = false;
            engageEvents.Invoke();
        }

        private void DisengageItem(GameObject canister)
        {
            canister.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            snapPointOccipied = false;
            disengageEvents.Invoke();
        }

        private void OnDisable()
        {
            if (snapPointOccipied && currentSnappedItem != null)
            {
                DisengageItem(currentSnappedItem);
            }
        }

    }
}

