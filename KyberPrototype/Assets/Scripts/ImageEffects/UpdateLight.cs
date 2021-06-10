using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class UpdateLight : MonoBehaviour
{
    public Transform mainLight;

    [SerializeField]
    private VisualEffect lightRay;
    private Vector3 sourceAngle;
    // Start is called before the first frame update
    void Awake()
    {
        sourceAngle = mainLight.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        sourceAngle = mainLight.eulerAngles;
        if (lightRay.GetVector3("LightAngle") != sourceAngle)
        {
            lightRay.SetVector3("LightAngle", sourceAngle);
        }
    }
}
