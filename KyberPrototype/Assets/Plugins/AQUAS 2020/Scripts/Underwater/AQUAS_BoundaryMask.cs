using UnityEngine;

namespace AQUAS
{
    //This script masks the first of 3 steps to mask out the water for split rendering
    public class AQUAS_BoundaryMask : MonoBehaviour
    {

        //Material for making the mask after rendering and null Material
        //The mask is rendered to a texture, the static boundary is the focus
        public Material mat;
        public Material matNull;
        public RenderTexture target;
        Camera cam;

        public GameObject nextCam;
        public GameObject boundaryObj;

        //Setting up materials, camera & shaders
        void Start()
        {

            mat = new Material(Shader.Find("Hidden/AQUAS/Utils/Boundary Mask"));
            matNull = new Material(Shader.Find("Hidden/AQUAS/Null"));

            target = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGBHalf);

            cam = GetComponent<Camera>();
            //cam.SetReplacementShader(Shader.Find("Hidden/AQUAS/Utils/Depth Only"), null);
            cam.targetTexture = target;
            cam.pixelRect = new Rect(0, 0, Screen.width, Screen.height);
        }

        //Make sure the masking is limited to the static boundary
        //Nothing else should be on that layer!
        private void OnPreCull()
        {
            if (boundaryObj == null)
            {
                return;
            }
            boundaryObj.layer = LayerMask.NameToLayer("Water");


        }

        //Move static boundary back to default layer when done.
        private void OnPostRender()
        {
            if (boundaryObj == null)
            {
                return;
            }
            boundaryObj.layer = LayerMask.NameToLayer("Default");
        }

        //Render to texture using the shader to create the first channel of the mask
        //Pass the mask to the next masking instance
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (boundaryObj == null)
            {
                Graphics.Blit(source, destination, matNull);

                nextCam.GetComponent<AQUAS_VolumeMask>().mat.SetTexture("_MaskTex", target);

                target.Release();

                return;
            }

            Graphics.Blit(source, destination, mat);

            nextCam.GetComponent<AQUAS_VolumeMask>().mat.SetTexture("_MaskTex", target);

            target.Release();
        }

        //Auto-adjust the size of the target texture on runtime
        //Unstable - Sometimes it will work nicely, sometimes it will crash Unity for no obvious reason, thus outcommented
        /*private void Update()
        {
            if (target.width != Screen.width || target.height != Screen.height)
            {
                target.Release();
                target.width = Screen.width;
                target.height = Screen.height;
            }
        }*/
    }
}

