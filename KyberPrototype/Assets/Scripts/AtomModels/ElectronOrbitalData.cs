using System.Collections;
using System.Collections.Generic;
using System;

namespace Kyber
{
    enum OrbitalShape
    {
        s, p, d, f
    }
    struct ElectronOrbital
    {
        public int energyLevel { get; set; }
        public OrbitalShape orbitalShape { get; set; }
        public int numElectrons { get; set; }
    }
    public class ElectronOrbitalData
    {
        private string[] fillOrder = { "1s", "2s", "2p", "3s", "3p", "3d", "4s", "4p", "4d", "4f", "5s", "5p", "5d", "5f", "6s", "6p", "6d", "7s", "7p" };
        private List<ElectronOrbital> orbitalData;
        private int currentElectrons = 0;
        public ElectronOrbitalData(int numElectrons)
        {
            orbitalData = new List<ElectronOrbital>(fillOrder.Length);

            if(numElectrons < 0)
                throw new ArgumentOutOfRangeException("number of electrons must be greater or equal to zero");

            PopulateOrbitalList(numElectrons);

        }

        private void PopulateOrbitalList(int numElectrons)
        {
            foreach (string orbital in fillOrder)
            {
                ElectronOrbital currentOrbital = new ElectronOrbital();
                currentOrbital.energyLevel = (int)char.GetNumericValue(orbital[0]);
                currentOrbital.orbitalShape = getOrbitalShape(orbital[1]);

                int remainingElectrons = numElectrons - currentElectrons;
                switch (currentOrbital.orbitalShape)
                {
                    case OrbitalShape.s:
                        AddOrbitalToList(currentOrbital, remainingElectrons, 2);
                        break;
                    case OrbitalShape.p:
                        AddOrbitalToList(currentOrbital, remainingElectrons, 6);
                        break;
                    case OrbitalShape.d:
                        AddOrbitalToList(currentOrbital, remainingElectrons, 10);
                        break;
                    case OrbitalShape.f:
                        AddOrbitalToList(currentOrbital, remainingElectrons, 14);
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddOrbitalToList(ElectronOrbital currentOrbital, int remainingElectrons, int maxElectrons)
        {
            if (remainingElectrons <= maxElectrons)
            {
                currentOrbital.numElectrons = remainingElectrons;
                currentElectrons += remainingElectrons;
                orbitalData.Add(currentOrbital);
            }
            else
            {
                currentOrbital.numElectrons = maxElectrons;
                currentElectrons += maxElectrons;
                orbitalData.Add(currentOrbital);
            }
        }

        private OrbitalShape getOrbitalShape(char c)
        {
            if (c == 's')
                return OrbitalShape.s;
            if (c == 'p')
                return OrbitalShape.p;
            if (c == 'd')
                return OrbitalShape.d;
            if (c == 'f')
                return OrbitalShape.f;
            throw new System.FormatException("Check fillOrder array in ElectronOrbitalData.cs, there's probably a typo. Can only be [integer] + [s, p, d, or f].");
        }

        public int[] GetOrbitalData(string orbital)
        {
            int index;
            if ((index = Array.IndexOf(fillOrder, orbital)) != -1)
            {
                ElectronOrbital currentOrbital = orbitalData[index];
                switch (currentOrbital.orbitalShape)
                {
                    // each index can be either 0, 1, or 2
                    // each index represents a spin up/spin down pair
                    case OrbitalShape.s:
                        return GenerateOrbitalData(1, currentOrbital.numElectrons);
                    case OrbitalShape.p:
                        return GenerateOrbitalData(3, currentOrbital.numElectrons);
                    case OrbitalShape.d:
                        return GenerateOrbitalData(5, currentOrbital.numElectrons);
                    case OrbitalShape.f:
                        return GenerateOrbitalData(7, currentOrbital.numElectrons);
                    default:
                        return new int[0];
                }
            }
            else
            {
                throw new KeyNotFoundException($"Invalid orbital {orbital}");
            }
        }
        private int[] GenerateOrbitalData(int indices, int numElectrons)
        {
            int[] electronData = new int[indices];
            for (int i = 0; i < indices; i++)
            {
                electronData[i] = 0;
            }
            for (int i = 0; i < numElectrons; i++)
            {
                electronData[i % indices] += 1;
            }
            return electronData;
        }

        public int[] GetBhorModelData()
        {
            int[] electronData = { 0, 0, 0, 0, 0, 0, 0 };
            foreach(ElectronOrbital orbital in orbitalData)
            {
                electronData[orbital.energyLevel - 1] += orbital.numElectrons;
            }
            return electronData;
        }
    }
}
