﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Valve.VR.InteractionSystem;


[RequireComponent(typeof(Rigidbody))]
public class AtomNucleus : MonoBehaviour
{
    [Header("Proton/Neutron resource filepaths")]
    public string protonResourceFilepath = "AtomComponents/Proton/Proton";
    public string neutronResourceFilepath = "AtomComponents/Neutron/Neutron";

    [Header("Nucleus data")]
    public int atomicNumber = 1;
    public int massNumber = 1;
    public float strongForce = 100f;
    public float inertia = 1f;

    [Header("Size")]
    public float nucleusDiameter = .1f;
    public float nucleusDiameterBuffer = .03f;

    private int numNeutrons;
    private float componentScale;

    private float[] packingEfficiency = { 0f, 1f, .25f, .29988f, .36326f, .35533f, .4264f, .40213f, .43217f, .44134f, .44005f, .45003f, .49095f };


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(nucleusDiameter + nucleusDiameterBuffer, nucleusDiameter + nucleusDiameterBuffer, nucleusDiameter + nucleusDiameterBuffer);
        componentScale = CalculateProtonNeutronRadius();
        SpawnNucleusComponents();
    }

    private void SpawnNucleusComponents()
    {
        GameObject proton = Resources.Load(protonResourceFilepath) as GameObject;
        GameObject neutron = Resources.Load(neutronResourceFilepath) as GameObject;

        if (!proton)
        {
            Debug.LogError($"Unable to find proton resource @ {protonResourceFilepath}");
        }
        else if (!neutron)
        {
            Debug.LogError($"Unable to find neutron resource @ {neutronResourceFilepath}");
        }
        else
        {
            numNeutrons = massNumber - atomicNumber;

            int currentNumProtons = 0, currentNumNeutrons = 0;
            while (currentNumProtons < atomicNumber || currentNumNeutrons < numNeutrons)
            {
                if (currentNumNeutrons < numNeutrons)
                {
                    //Debug.Log("Adding neutron");
                    currentNumNeutrons++;
                    AddElementToNucleus(neutron);
                }
                if (currentNumProtons < atomicNumber)
                {
                    //Debug.Log("Adding proton");
                    currentNumProtons++;
                    AddElementToNucleus(proton);
                }
            }
        }
    }

    private float CalculateProtonNeutronRadius()
    {
        // Debug.Log(transform.localScale);
        // Debug.Log(Mathf.Pow(.49365f / massNumber, (1f / 3f)));
        float packingEfficiencyVal = .56f;
        if (massNumber <= 12)
            packingEfficiencyVal = packingEfficiency[massNumber];
        return Mathf.Pow(packingEfficiencyVal / massNumber, (1f / 3f));
    }

    private void AddElementToNucleus(GameObject prefab)
    {
        GameObject newParticle = Instantiate(prefab, transform);
        newParticle.GetComponent<Rigidbody>().mass = .5f;
        newParticle.GetComponent<Rigidbody>().drag = inertia;
        newParticle.GetComponent<Rigidbody>().angularDrag = inertia;
        newParticle.GetComponent<Rigidbody>().useGravity = false;
        newParticle.GetComponent<Transform>().localScale = new Vector3(componentScale, componentScale, componentScale);

        SpringJoint sj = gameObject.AddComponent(typeof(SpringJoint)) as SpringJoint;
        sj.anchor = transform.position;
        sj.connectedBody = newParticle.GetComponent<Rigidbody>();
        sj.anchor = Vector3.zero;
        sj.connectedAnchor = Vector3.zero;
        sj.spring = strongForce;
        sj.damper = inertia;

        newParticle.GetComponent<Transform>().localPosition = new Vector3(Random.value * .02f - .01f, Random.value * .02f - .01f, Random.value * .02f - .01f);
    }
}