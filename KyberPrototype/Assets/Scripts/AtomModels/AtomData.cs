using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    [CreateAssetMenu(fileName = "AtomData", menuName = "Atom Data")]
    public class AtomData : ScriptableObject
    {
        public string scientificName;
        public int atomicNumber;
        public int massNumber;
        public int charge;  
    }
}

