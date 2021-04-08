using UnityEngine;

namespace AQUAS
{

    //This script renders a depth buffer to a render texture
    //The buffer can later be used to bypass a step in Unity's render pipeline
    public class AQUAS_RenderDepth : MonoBehaviour
    {

        [HideInInspector]
        public GameObject plane;

        Material material;
        RenderTexture target;

        string shaderPath;

        bool checkForGridProjection = true;

        // Use this for initialization
        void Start()
        {

            material = new Material(Shader.Find("Hidden/AQUAS/Utils/Screen Depth"));

            target = new RenderTexture(Screen.width/1, Screen.height/1, 32, RenderTextureFormat.ARGBHalf);
            target.filterMode = FilterMode.Bilinear;

            GetComponent<Camera>().targetTexture = target;
            //GetComponent<Camera>().SetReplacementShader(Shader.Find("Depth Only"), null);

            shaderPath = plane.GetComponent<Renderer>().sharedMaterials[0].shader.name;
            plane.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Hidden/AQUAS/Desktop/Front/Opaque");
            
        }

        private void Update()
        {
            //Sometimes the grid projection component arrives too late on the camera, and thus the masking cameras won't add it, even though they needs it if their parenting camera uses it
            //This block checks if the grid projection component has been added to the parenting camera later on and adds it too, if so
            if (GetComponentInParent<AQUAS_ProjectedGrid>() != null && GetComponents<AQUAS_ProjectedGrid>() != null && checkForGridProjection)
            {
                gameObject.AddComponent<AQUAS_ProjectedGrid>();
                GetComponent<AQUAS_ProjectedGrid>().waterplane = transform.parent.GetComponent<AQUAS_ProjectedGrid>().waterplane;
                GetComponent<AQUAS_ProjectedGrid>().waterLevel = transform.parent.GetComponent<AQUAS_ProjectedGrid>().waterLevel;
                checkForGridProjection = false;
            }
        }

        private void OnApplicationQuit()
        {
            plane.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find(shaderPath);
        }

        private void OnPreCull()
        {
            if (plane == null)
            {
                return;
            }
            plane.layer = LayerMask.NameToLayer("Water");
        }

        private void OnPostRender()
        {
            if (plane == null)
            {
                return;
            }

            plane.layer = LayerMask.NameToLayer("Default");
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, material);

            plane.GetComponent<Renderer>().sharedMaterial.SetTexture("_DeTex", target);

            target.Release();
        }
    }
}
