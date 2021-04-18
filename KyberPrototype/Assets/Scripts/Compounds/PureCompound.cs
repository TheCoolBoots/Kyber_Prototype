using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PureCompound : Compound
{
    bool stable { get; set; }
    List<Compound> decaysInto { get; set; }
    string chemicalFormula { get; set; }
    int moles { get; set; }
    int[] componentElements { get; set; }
}
