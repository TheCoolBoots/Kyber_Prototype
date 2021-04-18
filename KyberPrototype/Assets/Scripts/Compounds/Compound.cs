using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Compound
{
    GameObject compoundModel { get; set; }
    int compoundID { get; set; }
    float volume { get; set; }
    float mass { get; set; }
    float density { get; set; }
}
