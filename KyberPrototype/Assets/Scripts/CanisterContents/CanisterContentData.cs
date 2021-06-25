using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public enum States : int
    {
        Solid, Liquid, Gas, Plasma
    }

    public enum Types : int
    {
        Mixture, Molecule, Atom
    }


    [CreateAssetMenu(fileName = "CanisterContentData", menuName = "Canister Content Data")]
    public class CanisterContentData : ScriptableObject
    {

        [Header("For All Compounds")]
        public string commonName;
        public States state;
        public Types type;
        [Tooltip("g/cm^3 at 25C")]
        public float density = 1f;
        public float volume = 1f;
        [Tooltip("in Kelvin; 298.15K = room temperature")]
        public float temperature = 298.15f;
        [Tooltip("Kelvin @ 1 atm")]
        public float meltingPoint;
        [Tooltip("Kelvin @ 1 atm")]
        public float boilingPoint;
        [Tooltip("Kelvin @ 1 atm")]
        public float sublimationPoint;
        public string chemicalFormula;
        public string componentAtoms;
        public List<CanisterContentData> componentAtomsList;

        [Tooltip("Solids: a prefab of the model of that solid\nLiquids: the LiquidSphere prefab\nGasses: the EmissionLampBase prefab")]
        public GameObject basePrefab;

        // for liquids

        [Header("For Rendering Liquids")]
        [Tooltip("Can leave these blank if compound is solid or gas")]
        public float gradientScale = 2f;
        public float tilingSpeed = .4f;
        public float displacementScale = .05f;
        public float fresnelPower = 3f;
        public float indexOfRefraction = .05f;
        public Color tint;

        // for gasses
        [Tooltip("Can leave these blank if compound is solid or liquid")]
        [Header("For Rendering Gasses")]
        [ColorUsage(true, true)]
        public Color emissionColor;


        [Header("Element Data")]
        // Jean, add tooltips like these to the rest of the entries
        public string scientificName;
        public int atomicNumber;
        public int massNumber;
        public int charge;
        public int valanceElectron;
        public float atomicMass;
        public float electroNegativity;
        [Tooltip("picometers")]
        public float atomicRadius;
        [Tooltip("eV")]
        public float ionizationEnergy;


        [Header("Molecule Data")]
        public bool polarity;
        public string bondAngle;
        public string electronGeometry;
        public string molecularGeometry; 


    }
}

