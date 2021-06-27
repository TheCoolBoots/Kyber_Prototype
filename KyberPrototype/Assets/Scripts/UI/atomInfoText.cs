using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kyber;

    public class AtomInfoText : MonoBehaviour
    {
        [SerializeField] private AtomicAnalyzer atomicAnalyzer;

        [SerializeField] private TextMeshProUGUI atomName;
        [SerializeField] private TextMeshProUGUI atomSymbol;
        [SerializeField] private TextMeshProUGUI atomNumber;
        [SerializeField] private TextMeshProUGUI atomMass;
        [SerializeField] private TextMeshProUGUI atomValenceElcetrons;
        [SerializeField] private TextMeshProUGUI atomElectronegativity;
        [SerializeField] private TextMeshProUGUI atomRadius;
        [SerializeField] private TextMeshProUGUI atomIonizationEnergy;

        [SerializeField] private GameObject MoleculeInfo;
        [SerializeField] private GameObject AtomInfo;
        [SerializeField] private GameObject viewAtomButton;
        [SerializeField] private GameObject viewNewxtAtomButton;

        private CanisterContentData atomTempData;

        private int atomIndex = 0; 



        public void updateAtomInfo()
        {
            atomName.text = atomicAnalyzer.currentCompoundData.scientificName;
            atomSymbol.text = atomicAnalyzer.currentCompoundData.chemicalFormula;
            atomNumber.text = atomicAnalyzer.currentCompoundData.atomicNumber.ToString();
            atomMass.text = atomicAnalyzer.currentCompoundData.atomicMass.ToString();
            atomValenceElcetrons.text = atomicAnalyzer.currentCompoundData.valanceElectron.ToString();
            atomElectronegativity.text = atomicAnalyzer.currentCompoundData.valanceElectron.ToString();
            atomRadius.text = atomicAnalyzer.currentCompoundData.electroNegativity.ToString();
            atomIonizationEnergy.text = atomicAnalyzer.currentCompoundData.ionizationEnergy.ToString();
        }

       public void updatComponentAtomInfo()
       {
            MoleculeInfo.SetActive(false);
            AtomInfo.SetActive(true);

            atomTempData = atomicAnalyzer.currentCompoundData.componentAtomsList[0];

            atomName.text = atomTempData.scientificName; 
            atomSymbol.text = atomTempData.chemicalFormula;
            atomNumber.text = atomTempData.atomicNumber.ToString();
            atomMass.text = atomTempData.atomicMass.ToString();
            atomValenceElcetrons.text = atomTempData.valanceElectron.ToString();
            atomElectronegativity.text = atomTempData.valanceElectron.ToString();
            atomRadius.text = atomTempData.electroNegativity.ToString();
            atomIonizationEnergy.text = atomTempData.ionizationEnergy.ToString();
        }   

        public void updateNextComponentAtomInfo()
        {

            while (atomIndex < atomicAnalyzer.currentCompoundData.componentAtomsList.Count)
            {
                atomTempData = atomicAnalyzer.currentCompoundData.componentAtomsList[atomIndex + 1];

                atomName.text = atomTempData.scientificName;
                atomSymbol.text = atomTempData.chemicalFormula;
                atomNumber.text = atomTempData.atomicNumber.ToString();
                atomMass.text = atomTempData.atomicMass.ToString();
                atomValenceElcetrons.text = atomTempData.valanceElectron.ToString();
                atomElectronegativity.text = atomTempData.valanceElectron.ToString();
                atomRadius.text = atomTempData.electroNegativity.ToString();
                atomIonizationEnergy.text = atomTempData.ionizationEnergy.ToString();

                

            }

        }

       
    }

