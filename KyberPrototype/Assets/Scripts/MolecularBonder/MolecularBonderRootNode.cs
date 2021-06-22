using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyber;

public class MolecularBonderRootNode : MonoBehaviour
{
    [SerializeField] private GameObject rootBondingSite;
    [SerializeField] private GameObject[] outerBondingSites;

    [SerializeField] private VRSnapPoint rootAtomContainer;
    [SerializeField] private VRSnapPoint[] outerAtomContainers;

    private Atom rootAtom;
    private Atom[] outerAtoms;

    private int maxNumBondingSites;
    private int numBondingSites;
    private int maxNumBonds;
    private int numBonds;
    private int[] numBondsPerAtom;

    private void Awake()
    {
        outerAtoms = new Atom[4] { null, null, null, null };

        numBondsPerAtom = new int[4] { 0, 0, 0, 0};

        foreach (GameObject go in outerBondingSites)
        {
            go.SetActive(false);
        }
    }

    public void RootNodeStateChange()
    {
        numBonds = 0;
        if (rootAtomContainer.snapPointOccipied)
        {
            if((rootAtom = rootAtomContainer.currentSnappedItem.GetComponent<Atom>()) != null)
            {
                int valenceElectrons = GetNumValenceElectrons(rootAtom);
                if(valenceElectrons < 2)
                {
                    maxNumBondingSites = 1;
                    numBondingSites = 1;
                }
                else if(valenceElectrons >= 4)
                {
                    maxNumBondingSites = 8 - valenceElectrons;
                    numBondingSites = maxNumBondingSites;
                }
                else
                {
                    // ERROR: element cannot form covalent bond
                }
                SetNumBondingSites(numBondingSites);
            }
        }
        else
        {
            rootAtom = null;
            foreach (GameObject go in outerBondingSites)
            {
                go.SetActive(false);
            }
        }

    }

    private int GetNumValenceElectrons(Atom atom)
    {
        int[] orbitalData = atom.electronOrbitalData.GetBhorModelData();
        int valenceElectrons = 0;
        for(int i = 1; i < orbitalData.Length && orbitalData[i] > 0; i ++)
        {
            valenceElectrons = orbitalData[i];
        }
        print(valenceElectrons);
        return valenceElectrons;
    }

    public void OuterNodeStateChange()
    {
        numBonds = 0;
        for(int S = 0; S < 4; S++)
        {
            if (outerAtomContainers[S].snapPointOccipied)
            {
                if((outerAtoms[S] = outerAtomContainers[S].currentSnappedItem.GetComponent<Atom>()) != null)
                {
                    // enable bondControls[S]
                    // numBonds += bondControls[S].numBonds
                    // numBondsPerAtom[S] = bondControls[S].numBonds
                }
                else
                {
                    // disable bondControls[S]
                }
            }
            else
            {
                outerAtoms[S] = null;
                // disable bondControls[S]
            }
        }

        for(int i = 0; i < 4; i++)
        {
            // bondIndicators[i].setNumBonds(numBondsPerAtom[i])
            numBondingSites -= (numBondsPerAtom[i] - 1);
        }

        if(numBondingSites <= 0)
        {
            // ERROR: too many bonds for root atom
        }
        else
        {
            // if all atoms have full electron shells
                // success!, press generate compound
            // else
                // error, not all atoms have full valence shell
        }
    }
    private void SetNumBondingSites(int num)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < num)
            {
                outerBondingSites[i].SetActive(true);
            }
            else
            {
                outerBondingSites[i].SetActive(false);
            }
        }
    }
}
