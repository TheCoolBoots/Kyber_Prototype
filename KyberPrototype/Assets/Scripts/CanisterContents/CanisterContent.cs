using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kyber;

public class CanisterContent: MonoBehaviour
{
    public CanisterContentData _data;
    public TextMesh label;

    [HideInInspector]
    public string commonName;
    [HideInInspector]
    public States state;
    [HideInInspector]
    public double density;
    [HideInInspector]
    public double volume;
    [HideInInspector]
    public string chemicalFormula;
    [HideInInspector]
    public string componentAtoms;

    private GameObject contentsInstance;

    void Start()
    {
        if(_data != null)
        {
            commonName = _data.commonName;
            state = _data.state;
            density = _data.density;
            volume = _data.volume;
            chemicalFormula = _data.chemicalFormula;
            componentAtoms = _data.componentAtoms;

            InstantiateContentPrefab();
        }
    }

    private void InstantiateContentPrefab()
    {
        contentsInstance = Instantiate(_data.basePrefab, transform);
        label.text = $"{commonName}: {state}";
        switch (state)
        {
            case (States.Liquid):
                MeshRenderer sphereRenderer = contentsInstance.GetComponent<MeshRenderer>();
                sphereRenderer.material.SetFloat("_GradientScale", _data.gradientScale);
                sphereRenderer.material.SetFloat("_TilingSpeed", _data.tilingSpeed);
                sphereRenderer.material.SetFloat("_DisplacementScale", _data.displacementScale);
                sphereRenderer.material.SetFloat("_FresnelPower", _data.fresnelPower);
                sphereRenderer.material.SetFloat("_IOR", _data.indexOfRefraction);
                sphereRenderer.material.SetColor("_LiquidTint", _data.tint);
                break;
            case (States.Gas):
                contentsInstance.GetComponent<SetEmissionColor>().SetEmissionColorTo(_data.emissionColor);
                break;
            case (States.Solid):
                break;
            default:
                break;
        }
    }
}
