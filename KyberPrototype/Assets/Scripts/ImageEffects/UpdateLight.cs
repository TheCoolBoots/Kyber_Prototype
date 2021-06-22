using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEditor;

[ExecuteInEditMode]
public class UpdateLight : MonoBehaviour
{
    public Light mainLight;
    public Light oceanLight;
    public Material skybox;

    [SerializeField]
    private VisualEffect lightRay;
    private Transform mainLightPos;
    private Vector3 sourceAngle;
    private Vector3 cameraPos;
    private Vector3 lastCameraPos;
    private float depthBound = 50f;
    // Start is called before the first frame update
    void Awake()
    {
        mainLightPos = mainLight.transform;
        sourceAngle = mainLightPos.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        sourceAngle = mainLightPos.eulerAngles;
        if (lightRay.GetVector3("LightAngle") != sourceAngle)
        {
            lightRay.SetVector3("LightAngle", sourceAngle);
        }

        if (Camera.current != null)
        {
            cameraPos = Camera.current.transform.position;
            //cameraPos = SceneView.currentDrawingSceneView.camera.transform.position;
            //print(cameraPos);
        }
        else if (Application.isFocused)
        {
            cameraPos = Camera.main.transform.position;
        }
        
        if (cameraPos != lastCameraPos)
        {
            float mainLightIntensity = Mathf.Clamp01((depthBound + cameraPos.y) / depthBound)*1.4f;
            mainLight.intensity = mainLightIntensity;
            float oceanLightIntensity = Mathf.Clamp01((-cameraPos.y) / depthBound);
            oceanLight.intensity = oceanLightIntensity;
            lastCameraPos = cameraPos;
            
            //skybox.shader.
            //mainLight.intensity = cameraPos.y
        }
        //print(Application.isFocused);
    }
}
