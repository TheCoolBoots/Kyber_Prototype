using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public enum States : int
    {
        Solid, Liquid, Gas, Plasma
    }

    [CreateAssetMenu(fileName = "CanisterContentData", menuName = "Canister Content Data")]
    public class CanisterContentData : ScriptableObject
    {

        [Header("For All Compounds")]
        public string commonName;
        public States state;
        public double density = 1f;
        public double volume = 1f;
        public string chemicalFormula;
        public string componentAtoms;

        [Tooltip("Solids: a prefab of the model of that solid\nLiquids: the LiquidSphere prefab\nGasses: the EmissionLampBase prefab")]
        public GameObject basePrefab;

        // for liquids
        [Tooltip("Can leave these blank if compound is solid or gas")]
        [Header("For Rendering Liquids")]
        public float gradientScale = 2f;
        public float tilingSpeed = .4f;
        public float displacementScale = .05f;
        public float fresnelPower = 3f;
        public float indexOfRefraction = .05f;
        public Color tint;

        // for gasses
        [Tooltip("Can leave these blank if compound is solid or gas")]
        [Header("For Rendering Gasses")]
        [ColorUsage(true, true)]
        public Color emissionColor;


        [Header("Element Data")]
        [Tooltip("The number of protons in the element")] // Jean, add tooltips like these to the rest of the entries
        public int atomicNumber;
        public int massNumber;
        public float atomicMass;
        public float electroNegativity;
        public float atomicRadius;
        public float ionizationEnergy;
        public float meltingPoint;
        public float boilingPoint;

    }
}

