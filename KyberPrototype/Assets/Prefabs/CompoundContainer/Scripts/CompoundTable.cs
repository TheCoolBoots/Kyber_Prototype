using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public enum CompoundIDs : int
    {
        Empty = 0,
        H = 1,
        C = 6,
        N = 7,
        O = 8,
        PureWater = 119,
        CarbonDioxide = 120,
    }

    public enum States : int
    {
        Solid, Liquid, Gas, Plasma
    }

    
    public class CompoundTable : MonoBehaviour
    {
        private float PLACEHOLDER = 0.0f;
        public Dictionary<CompoundIDs, CompoundData> compoundTable;
        private void Awake()
        {
            compoundTable = new Dictionary<CompoundIDs, CompoundData>(100);
            // CompoundIDs compoundID, States state, string commonName, float density, float volume, bool stable, string chemicalFormula
            compoundTable.Add(CompoundIDs.H, new CompoundData(States.Gas, "Hydrogen", PLACEHOLDER, PLACEHOLDER, true, "H2", "CompoundSpheres/AirSphere/AirSphere", .4f, "H"));
            compoundTable.Add(CompoundIDs.C, new CompoundData(States.Solid, "Graphite", PLACEHOLDER, PLACEHOLDER, true, "C", "CompoundSpheres/Graphite/Graphite", 3f, "C"));
            compoundTable.Add(CompoundIDs.N, new CompoundData(States.Gas, "Nitrogen", PLACEHOLDER, PLACEHOLDER, true, "N2", "CompoundSpheres/AirSphere/AirSphere", 1.0f, "N"));
            compoundTable.Add(CompoundIDs.O, new CompoundData(States.Gas, "Oxygen", PLACEHOLDER, PLACEHOLDER, true, "O2", "CompoundSpheres/AirSphere/AirSphere", 1.0f, "O"));
            compoundTable.Add(CompoundIDs.CarbonDioxide, new CompoundData(States.Gas, "Carbon Dioxide", PLACEHOLDER, PLACEHOLDER, true, "CO2", "CompoundSpheres/AirSphere/AirSphere", 1.0f, "C O"));
            compoundTable.Add(CompoundIDs.PureWater, new CompoundData(States.Liquid, "Pure Water", PLACEHOLDER, PLACEHOLDER, true, "H2O", "CompoundSpheres/WaterSphere/WaterSphere", .8f, "H O"));
        }
    }
}

