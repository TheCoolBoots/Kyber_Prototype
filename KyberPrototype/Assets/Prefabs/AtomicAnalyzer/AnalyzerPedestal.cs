using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Kyber;

public class AnalyzerPedestal : MonoBehaviour
{
    public GameObject canisterShadowPrefab;
    public Transform snapPoint;

    private Interactable canisterInteractible;
    private GameObject canisterShadow;
    private void Start()
    {
        if(canisterShadowPrefab != null)
        {
            canisterShadow = Instantiate(canisterShadowPrefab, snapPoint);
            canisterShadow.SetActive(false);
        }
        else
        {
            Debug.LogError("Forgot to assign canisterShadow to something");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        Compound otherCompound = other.gameObject.GetComponent<Compound>();
        if (otherCompound != null)
        {
            canisterInteractible = other.gameObject.GetComponent<Interactable>();
            if (canisterInteractible.attachedToHand)
            {
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                canisterShadow.SetActive(true);
            }
            else if(canisterShadow.activeSelf)
            {
                canisterShadow.SetActive(false);
                other.gameObject.transform.position = snapPoint.position;
                other.gameObject.transform.rotation = snapPoint.rotation;

                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        // Debug.Log($"Other collider name: {other.gameObject.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        canisterShadow.SetActive(false);
        canisterInteractible = null;
    }


    /*

    if object colliding is compoundCanister
        if hand is attatched && shadow is uninstantiated
            create shadow
        else if hand is not attached
            if shadow exists
                kill shadow
            snap canister to snap point       

    */
}
