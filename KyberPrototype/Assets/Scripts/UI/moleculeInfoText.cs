using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoleculeInfoText : MonoBehaviour
{
    [SerializeField] private AtomicAnalyzer atomicAnalyzer;

    [SerializeField] private TextMeshProUGUI moleculeName;
    [SerializeField] private TextMeshProUGUI moleculeFormula;
    [SerializeField] private TextMeshProUGUI moleculeDensity;
    [SerializeField] private TextMeshProUGUI moleculePolrity;
    [SerializeField] private TextMeshProUGUI moleculeBondAngle;
    [SerializeField] private TextMeshProUGUI moleculeComponentAtoms;
    [SerializeField] private TextMeshProUGUI moleculeElectronGeo;
    [SerializeField] private TextMeshProUGUI moleculeMolecularGeo;


    public void updateMoleculInfo()
    {
        moleculeName.text = atomicAnalyzer.currentCompoundData.commonName;
        moleculeFormula.text = atomicAnalyzer.currentCompoundData.chemicalFormula;
        moleculeDensity.text = atomicAnalyzer.currentCompoundData.density.ToString();
        moleculePolrity.text = checkPolarity(atomicAnalyzer.currentCompoundData.polarity);
        moleculeBondAngle.text = atomicAnalyzer.currentCompoundData.bondAngle;
        moleculeComponentAtoms.text = atomicAnalyzer.currentCompoundData.componentAtoms;
        moleculeElectronGeo.text = atomicAnalyzer.currentCompoundData.electronGeometry;
        moleculeMolecularGeo.text = atomicAnalyzer.currentCompoundData.molecularGeometry;

    }

    string checkPolarity(bool moleculePolarity)
    {
        string polarity = "n/a";
        if(moleculePolarity == true)
        {
            polarity = "polar";
        }
        else
        {
            polarity = "non polar";
        }

        return polarity;
    }
}
