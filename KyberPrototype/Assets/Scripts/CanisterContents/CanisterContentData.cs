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
        [Header("For Liquids Only")]
        public float gradientScale = 2f;
        public float tilingSpeed = .4f;
        public float displacementScale = .05f;
        public float fresnelPower = 3f;
        public float indexOfRefraction = .05f;
        public Color tint;

        // for gasses
        [Header("For Gasses Only")]
        [ColorUsage(true, true)]
        public Color emissionColor;

    }
}

