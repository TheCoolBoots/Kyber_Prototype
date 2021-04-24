using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyber
{
    public enum CompoundIDs : int
    {
        H = 1,
        He = 2,
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
        public Dictionary<CompoundIDs, CompoundData> compoundTable;
        private void Awake()
        {
            compoundTable = new Dictionary<CompoundIDs, CompoundData>(100);
            // CompoundIDs compoundID, States state, string commonName, float density, float volume, bool stable, string chemicalFormula
            compoundTable.Add(CompoundIDs.H, new CompoundData(States.Gas, "Hydrogen Gas", .002, 300, true, "H", "AirSphere/AirSphere", .04f, "H"));
            // compoundTable.Add(CompoundIDs.C, new CompoundData(States.Solid, "Carbon", .1, 300, true, "C(1)"));
            // compoundTable.Add(CompoundIDs.N, new CompoundData(States.Gas, "Nitrogen Gas", .02, 300, true, "N(1)"));
            compoundTable.Add(CompoundIDs.PureWater, new CompoundData(States.Liquid, "Water", 1.0, 1.0, true, "H2O", "WaterSphere", .95f, "H O"));
        }
    }
}

