using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyber;

public class MolecularBonderRootNode : MonoBehaviour
{
    [SerializeField] private GameObject rootGO;
    [SerializeField] private GameObject northGO;
    [SerializeField] private GameObject southGO;
    [SerializeField] private GameObject eastGO;
    [SerializeField] private GameObject westGO;

    [SerializeField] private VRSnapPoint rootSnapPoint;
    [SerializeField] private VRSnapPoint northSnapPoint;
    [SerializeField] private VRSnapPoint southSnapPoint;
    [SerializeField] private VRSnapPoint eastSnapPoint;
    [SerializeField] private VRSnapPoint westSnapPoint;

    [SerializeField] private MeshRenderer rootIndicator;
    [SerializeField] private MeshRenderer northIndicator;
    [SerializeField] private MeshRenderer southIndicator;
    [SerializeField] private MeshRenderer eastIndicator;
    [SerializeField] private MeshRenderer westIndicator;

    private GameObject[] bondingSites;
    private MeshRenderer[] indicators;

    private void Awake()
    {
        bondingSites = new GameObject[4];
        bondingSites[0] = westGO;
        bondingSites[1] = northGO;
        bondingSites[2] = eastGO;
        bondingSites[3] = southGO;

        indicators = new MeshRenderer[4];
        indicators[0] = westIndicator;
        indicators[1] = northIndicator;
        indicators[2] = eastIndicator;
        indicators[3] = southIndicator;
    }

    // Start is called before the first frame update
    void Start()
    {
        resetBondingSites();
    }

    public void UpdateRootNode()
    {
        if (rootSnapPoint.snapPointOccipied)
        {
            rootIndicator.enabled = false;
            Atom currentAtom;
            if((currentAtom = rootSnapPoint.currentSnappedItem.GetComponent<Atom>()) != null)
            {
                AtomData currentAtomData = currentAtom.atomData;
                ElectronOrbitalData orbitalData = currentAtom.electronOrbitalData;
                int[] bhorModelData = orbitalData.GetBhorModelData();

                int valenceElectrons = 0;

                for(int i = 0; i < bhorModelData.Length && bhorModelData[i] > 0; i++)
                {
                    valenceElectrons = bhorModelData[i];
                }

                if(valenceElectrons < 4)
                {
                    SetNumBondingSites(1);
                }
                else
                {
                    // NOTE: only works with atoms that have <= 8 valence electrons
                    SetNumBondingSites(8 - valenceElectrons);
                }
            }
        }
        else
        {
            rootIndicator.enabled = true;
            resetBondingSites();
        }
    }

    void SetNumBondingSites(int num)
    {
        for(int i = 0; i < 4; i++)
        {
            if(i < num)
            {
                bondingSites[i].SetActive(true);
                indicators[i].enabled = true;
            }
            else
            {
                bondingSites[i].SetActive(false);
                indicators[i].enabled = false;
            }
        }
    }

    void resetBondingSites()
    {
        northGO.SetActive(false);
        southGO.SetActive(false);
        eastGO.SetActive(false);
        westGO.SetActive(false);

        indicators[0].enabled = false;
        indicators[1].enabled = false;
        indicators[2].enabled = false;
        indicators[3].enabled = false;
    }
}
