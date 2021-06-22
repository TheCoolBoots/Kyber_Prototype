using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Kyber;

namespace Tests
{
    public class OrbitalDataTest
    {
    
        [Test]
        public void OrbitalDataTestSimplePasses()
        {
            ElectronOrbitalData testData = new ElectronOrbitalData(9);
            int[] testArray = { 2 };
            Assert.AreEqual(testData.GetOrbitalData("1s"), testArray);
            int[] testArray1 = { 2 };
            Assert.AreEqual(testData.GetOrbitalData("2s"), testArray1);
            int[] testArray2 = { 2, 2, 1 };
            Assert.AreEqual(testData.GetOrbitalData("2p"), testArray2);
            // Use the Assert class to test conditions
        }


    }
}
