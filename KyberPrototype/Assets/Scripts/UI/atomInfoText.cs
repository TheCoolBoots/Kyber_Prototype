using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


        public void updateAtomInfo()
        {
            atomName.text = atomicAnalyzer.currentCompoundData.commonName;
            atomSymbol.text = atomicAnalyzer.currentCompoundData.chemicalFormula;
            atomNumber.text = atomicAnalyzer.currentCompoundData.atomicNumber.ToString();
            atomMass.text = atomicAnalyzer.currentCompoundData.atomicMass.ToString();
            atomValenceElcetrons.text = atomicAnalyzer.currentCompoundData.valanceElectron.ToString();
            atomElectronegativity.text = atomicAnalyzer.currentCompoundData.valanceElectron.ToString();
            atomRadius.text = atomicAnalyzer.currentCompoundData.electroNegativity.ToString();
            atomIonizationEnergy.text = atomicAnalyzer.currentCompoundData.ionizationEnergy.ToString();


    }
    }

