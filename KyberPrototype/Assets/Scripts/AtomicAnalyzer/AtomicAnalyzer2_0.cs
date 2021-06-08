﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyber;
using Valve.VR.InteractionSystem;

public class AtomicAnalyzer2_0 : MonoBehaviour
{
    /*private enum AtomModelTypes
    {
        Simple, Bhor, ElectronOrbitals
    }*/

    
    //[SerializeField] private GameObject bhorElectronPrefab;
    //[SerializeField] private GameObject protonPrefab;
    //[SerializeField] private GameObject electronPrefab;


    //[SerializeField] private AtomModelTypes spawnAtomType;

    //

    //[SerializeField] private bool active = false;           // make these two read-only
    //[SerializeField] private bool analyzerEnabled = false;  

    //[SerializeField] private List<GameObject> currentAtoms;



    [Header("Analyzer Pedestal Variables")]
    [SerializeField] private GameObject canisterShadowPrefab;
    [SerializeField] private Transform canisterSnapPoint;
    [SerializeField] private float snapPointRadius;
    private GameObject canisterShadowInstance;
    private SphereCollider pedestalSnapCollider;
    private bool pedestalOccupied;
    private GameObject currentEngagedCanister;

    [Header("Atom Spawner Variables")]
    [SerializeField] private Transform atomSpawnPoint;
    [SerializeField] private float spawnerSpacing = .3f;
    [SerializeField] private GameObject atomSpawnerPrefab;
    private List<GameObject> activeAtomSpawners;

    [Header("Atomic Analyzer Variables")]
    private bool active = false;
    private CanisterContentData currentCompoundData;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAnalyzerPedestal();
        activeAtomSpawners = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**********************************************************************
     * Analyzer Pedestal Code
     **********************************************************************/
    private void InitializeAnalyzerPedestal()
    {
        pedestalSnapCollider = gameObject.AddComponent<SphereCollider>();
        pedestalSnapCollider.center = canisterSnapPoint.localPosition;
        pedestalSnapCollider.radius = snapPointRadius;
        pedestalSnapCollider.isTrigger = true;

        canisterShadowInstance = Instantiate(canisterShadowPrefab, transform);
        canisterShadowInstance.transform.localPosition = canisterSnapPoint.localPosition;
        canisterShadowInstance.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pedestalOccupied)
            canisterShadowInstance.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!pedestalOccupied && IsValidCanister(other))
        {
            Interactable otherInteractable = other.GetComponent<Interactable>();

            if (otherInteractable.attachedToHand)
            {
                canisterShadowInstance.SetActive(true);
            }
            else
            {
                EngageCanister(other);
            }
        }
        else if (pedestalOccupied && currentEngagedCanister.GetComponent<Interactable>().attachedToHand)
        {
            DisengageCanister(currentEngagedCanister);
        }

    }

    private void EngageCanister(Collider other)
    {
        canisterShadowInstance.SetActive(false);
        other.gameObject.transform.position = canisterSnapPoint.position;
        other.gameObject.transform.rotation = canisterSnapPoint.rotation;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        pedestalOccupied = true;
        currentEngagedCanister = other.gameObject;
        currentCompoundData = other.gameObject.GetComponent<CanisterContent>()._data;
    }

    private void DisengageCanister(GameObject canister)
    {
        canisterShadowInstance.SetActive(true);
        canister.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        pedestalOccupied = false;
        currentCompoundData = null;
    }

    private bool IsValidCanister(Collider other)
    {
        return (other.gameObject.GetComponent<Interactable>() != null && other.gameObject.GetComponent<CanisterContent>() != null);
    }

    /**********************************************************************
     * Atom Generation Code
     **********************************************************************/
    private void CreateAtomSpawners()
    {
        string[] atoms = currentCompoundData.componentAtoms.Split(' '); // "H O" -> ["H", "O"]

        // for each element in the atomComponents list, create an atom spawner
        for (int i = 0; i < atoms.Length; i++)
        {
            GameObject spawner = Instantiate(atomSpawnerPrefab);
            spawner.GetComponent<AtomSpawner>().elementCharacter = atoms[i];
            spawner.GetComponent<AtomSpawner>().parent = atomSpawnPoint;

            // space out each atom spawner with spawnerSpacing between them
            spawner.GetComponent<AtomSpawner>().spawnPosition = new Vector3(atomSpawnPoint.localPosition.x, atomSpawnPoint.localPosition.y, atomSpawnPoint.localPosition.z - i * spawnerSpacing);
            spawner.GetComponent<AtomSpawner>().ActivateSpawner();
            activeAtomSpawners.Add(spawner);
        }
    }

    private void ResetAnalyzer()
    {
        foreach (GameObject spawner in activeAtomSpawners)
        {
            Destroy(spawner);
        }
        activeAtomSpawners = new List<GameObject>(5);

        active = false;
    }

    /**********************************************************************
     * Atomic Analyzer Code
     **********************************************************************/
    public void AnalyzeCompound()
    {
        if (currentEngagedCanister != null)
        {
            // send data to AnalyzerScreen and display it
            Debug.Log("Analyzing");

            ResetAnalyzer();

            // using compound data, create spawners of specified elements
            if (currentCompoundData != null)
            {
                Debug.Log("Creating AtomSpawners");
                CreateAtomSpawners();

                // set active flag to true
                active = true;
            }

        }
    }
}