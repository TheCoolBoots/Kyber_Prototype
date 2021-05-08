using UnityEngine;

namespace AQUAS
{

    //This script masks the second of 3 steps to mask out the water for split rendering
    public class AQUAS_VolumeMask : MonoBehaviour
    {

        //Material for making the mask after rendering and null Material
        //The mask is rendered to a texture, the backface of the waterplane is the focus
        public Material mat;
        Material matNull;
        RenderTexture target;
        Camera cam;

        public GameObject nextCam;
        public GameObject waterObj;

        public Renderer rend;
        public Shader[] waterShaders;

        public bool checkForGridProjection = true;

        //Setting up materials, camera & shaders
        //Render the waterplane with a simple shader that culls the front face
        void Start()
        {

            mat = new Material(Shader.Find("Hidden/AQUAS/Utils/Volume Mask"));
            matNull = new Material(Shader.Find("Hidden/AQUAS/Null"));

            target = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGBHalf);

            cam = GetComponent<Camera>();
            //cam.SetReplacementShader(Shader.Find("AQUAS/Utils/Depth Only Back"), null);
            cam.targetTexture = target;
            cam.pixelRect = new Rect(0, 0, Screen.width, Screen.height);

            waterShaders = new Shader[2];
        }

        //Make sure the masking is limited to the water plane
        //Nothing else should be on that layer!
        private void OnPreCull()
        {
            if (waterObj == null)
            {
                return;
            }
            waterObj.layer = LayerMask.NameToLayer("Water");

            foreach (Material mat in rend.sharedMaterials)
            {
                mat.shader = Shader.Find("Hidden/AQUAS/Utils/Depth Only Back");

                if (transform.parent.GetComponent<AQUAS_ProjectedGrid>() == null)
                {
                    mat.SetFloat("_ProjectGrid", 0);
                }
                else
                {
                    mat.SetFloat("_ProjectGrid", 1);
                }
            }

            //cam.SetReplacementShader(waterObj.GetComponent<Renderer>().sharedMaterials[1].shader, null);
        }

        //Move water plane back to default layer when done.
        private void OnPostRender()
        {
            if (waterObj == null)
            {
                return;
            }
            waterObj.layer = LayerMask.NameToLayer("Default");

            rend.sharedMaterials[0].shader = waterShaders[0];
            rend.sharedMaterials[1].shader = waterShaders[1];
        }

        //Render to texture using the shader to create the second channel of the mask
        //Pass the mask to the next masking instance
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (waterObj == null)
            {
                Graphics.Blit(source, destination, matNull);

                nextCam.GetComponent<AQUAS_FrontFaceMask>().mat.SetTexture("_MaskTex", target);

                target.Release();

                return;
            }

            Graphics.Blit(source, destination, mat);

            nextCam.GetComponent<AQUAS_FrontFaceMask>().mat.SetTexture("_MaskTex", target);

            target.Release();
        }

        //Auto-adjust the size of the target texture on runtime
        //Unstable - Sometimes it will work nicely, sometimes it will crash Unity for no obvious reason, thus outcommented
        private void Update()
        {
            /*if (target.width != Screen.width || target.height != Screen.height)
            {
                target.Release();
                target.width = Screen.width;
                target.height = Screen.height;
            }

            if (transform.parent.GetComponent<AQUAS_ProjectedGrid>() != null && checkForGridProjection)
            {
                gameObject.AddComponent<AQUAS_ProjectedGrid>();
                GetComponent<AQUAS_ProjectedGrid>().waterplane = transform.parent.GetComponent<AQUAS_ProjectedGrid>().waterplane;
                GetComponent<AQUAS_ProjectedGrid>().waterLevel = transform.parent.GetComponent<AQUAS_ProjectedGrid>().waterLevel;
                checkForGridProjection = false;
            }*/
        }
    }
}
