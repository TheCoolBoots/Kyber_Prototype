using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public class Atom : MonoBehaviour
    {
        private enum displayModes
        {
            BohrModel,
            ElectronClouds,
            OrbitalElectronsInClouds
        }

        public AtomData atomData;
        public bool onlyShowValenceElectrons;

        private ElectronOrbitalData electronOrbitalData;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

