using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{

    public class CompoundData
    {

        public States state { get; set; }
        public string commonName { get; set; }
        public double density { get; set; }
        public double volume { get; set; }
        public bool stable { get; set; }
        public string chemicalFormula { get; set; }
        public string resourceFilepath { get; set; }
        public float modelScale { get; set; }
        public string atomComponents { get; set; }

        public CompoundData(States state, string commonName, double density, double volume, bool stable, 
            string chemicalFormula, string resourceFilepath, float modelScale, string atomComponents)
        {
            this.state = state;
            this.commonName = commonName;
            this.density = density;
            this.volume = volume;
            this.stable = stable;
            this.chemicalFormula = chemicalFormula;
            this.resourceFilepath = resourceFilepath;
            this.modelScale = modelScale;
            this.atomComponents = atomComponents;
        }
    }
 
}

