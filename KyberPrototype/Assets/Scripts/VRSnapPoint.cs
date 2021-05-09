using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Kyber
{
    public class VRSnapPoint : MonoBehaviour
    {
        public Transform snapPoint;
        public bool showColliderBounds = true;
        public float colliderRadius = .5f;
        public Material colliderBoundsMaterial;
        public bool snapPointOccipied = false;
        public GameObject tempItemShadow;

        [HideInInspector]
        public GameObject currentSnappedItem;

        private SphereCollider colliderBounds;
        private GameObject colliderBoundsIndicator;

        private void Start()
        {

            // add sphere collider to current game object, centered @snapPoint, have it act as trigger
            colliderBounds = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
            colliderBounds.isTrigger = true;
            colliderBounds.radius = colliderRadius;
            colliderBounds.center = Vector3.zero;

            // create a sphere that shows where the snap zone is
            if (showColliderBounds)
            {
                colliderBoundsIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                colliderBoundsIndicator.GetComponent<Renderer>().material = colliderBoundsMaterial;
                Destroy(colliderBoundsIndicator.GetComponent<SphereCollider>());
                colliderBoundsIndicator.transform.localScale = new Vector3(colliderRadius * 2, colliderRadius * 2, colliderRadius * 2);
                colliderBoundsIndicator.transform.position = snapPoint.position;
                colliderBoundsIndicator.SetActive(false);
            }


        }

        /*        private void OnTriggerEnter(Collider other)
                {
                    if (showColliderBounds && !snapPointOccipied)
                    {
                        colliderBoundsIndicator.SetActive(true);
                    }
                }*/

        private void OnTriggerExit(Collider other)
        {
            colliderBoundsIndicator.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            // if snap point is empty, and component colliding with snap point has a Interactable && Throwable component
            if (!snapPointOccipied && other.GetComponent<Interactable>() != null && other.GetComponent<Throwable>() != null)
            {
                if (!colliderBoundsIndicator.activeInHierarchy)
                {
                    colliderBoundsIndicator.SetActive(true);
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

        private void SetItemShadow(Collider other)
        {
            throw new NotImplementedException();
        }

        private void EngageItem(Collider other)
        {
            // Debug.Log("Engaging item");
            other.gameObject.transform.position = snapPoint.position;
            other.gameObject.transform.rotation = snapPoint.rotation;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            snapPointOccipied = true;
            currentSnappedItem = other.gameObject;
            colliderBoundsIndicator.SetActive(false);
        }

        private void DisengageItem(GameObject canister)
        {
            canister.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            snapPointOccipied = false;
        }

    }
}

