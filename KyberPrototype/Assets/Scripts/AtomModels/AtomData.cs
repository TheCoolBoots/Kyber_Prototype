using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public class AtomData : ScriptableObject
    {
        public string scientificName;
        public int atomicNumber;
        public int massNumber;
        public float atomicMass;
        public List<int> isotopes;
        public int charge;
    }
}

