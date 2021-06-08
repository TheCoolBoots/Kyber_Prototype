using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Kyber
{
    public class atomInfoText : MonoBehaviour
    {
        public AnalyzerPedestal analyzerPedestal;

        public TextMeshProUGUI atomName;
        public TextMeshProUGUI atomSymbol;
        public TextMeshProUGUI atomNumber;
        public TextMeshProUGUI atomMass;
        public TextMeshProUGUI atomValenceElcetrons;
        public TextMeshProUGUI atomRadius;
        public TextMeshProUGUI atomIonizationEnergy;

        private AtomicAnalyzer canisterPedestal;
        private CanisterContentData data;

        // Start is called before the first frame update
        void Start()
        {
            atomName.text = "testing";
            atomName.fontSize = 0.01f;
        }

        // Update is called once per frames
        void Update()
        {

        }

        void updateAtomInfo()
        {

            canisterPedestal = GameObject.Find("canisterPedestal").GetComponent<AtomicAnalyzer>();

            data = canisterPedestal.GetComponent<AnalyzerPedestal>().currentCanister.GetComponent<CanisterContent>()._data;
        }
    }
}
