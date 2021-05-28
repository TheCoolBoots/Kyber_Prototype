using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhorModelRing : MonoBehaviour
{
    public int numElectrons = 0;
    public float electronSpeed = 1f;
    public float ringRadius = .5f;
    public float electronScale = .2f;

    [Space]
    [Tooltip("Enable/disable electron movement")]
    public bool orbiting = true;

    [Space]
    [HideInInspector]
    public GameObject electronPrefab;

    private List<GameObject> electrons = new List<GameObject>();
    public bool active = false;
    private bool lastFrameOrbiting = false;

    void Update()
    {
        if (active)
        {
            if (orbiting)
            {
                foreach (GameObject electron in electrons)
                    electron.transform.RotateAround(transform.TransformPoint(Vector3.zero), transform.TransformDirection(Vector3.up), electronSpeed * Time.deltaTime);
            }
            else
            {
                // if ring goes from orbiting to not orbiting, reset position of electrons
                if(lastFrameOrbiting)
                    ResetElectronPositions();
            }
        }
        lastFrameOrbiting = orbiting;
    }

    public void InitializeElectrons()
    {
        for (int i = 0; i < numElectrons; i++)
        {
            GameObject currentElectron = Instantiate(electronPrefab, transform);
            currentElectron.transform.localScale = new Vector3(electronScale, electronScale, electronScale);
            currentElectron.layer = 10;
            electrons.Add(currentElectron);
        }
        active = true;

        ResetElectronPositions();
    }
    private void ResetElectronPositions()
    {
        if(numElectrons > 2 && numElectrons <= 8)
        {
            float angleBetweenElectrons = Mathf.PI / 2;
            for (int i = 0; i < numElectrons; i++)
            {
                if(i < 4)
                {
                    electrons[i].transform.localPosition = new Vector3(Mathf.Cos((angleBetweenElectrons * i) - (Mathf.PI / 12)), 0, 
                        Mathf.Sin((angleBetweenElectrons * i) - (Mathf.PI / 12))) * ringRadius;
                }
                else
                {
                    electrons[i].transform.localPosition = new Vector3(Mathf.Cos((angleBetweenElectrons * i) + (Mathf.PI / 12)), 0, 
                        Mathf.Sin((angleBetweenElectrons * i) + (Mathf.PI / 12))) * ringRadius;
                }
            }
        }
        else
        {
            float angleBetweenElectrons = 2 * (float)Math.PI / numElectrons;
            for (int i = 0; i < numElectrons; i++)
            {
                electrons[i].transform.localPosition = new Vector3(Mathf.Cos(angleBetweenElectrons * i), 0, Mathf.Sin(angleBetweenElectrons * i)) * ringRadius;
            }
        }
    }
}
