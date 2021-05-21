using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEmissionColor : MonoBehaviour
{
    public MeshRenderer emissionCylinderMeshRenderer;

    public void SetEmissionColorTo(Color color)
    {
        emissionCylinderMeshRenderer.material.SetColor("_EmissionColor", color);
    }


}
