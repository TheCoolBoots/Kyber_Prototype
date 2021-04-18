using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CompoundMixture : Compound
{
    Dictionary<Compound, float> percentComposition { get; set; }
}
