using UnityEngine;

namespace AQUAS
{

    //Grabbing the screen color in any shader will only work on objects that are on the geometry queue - thus it will fail to capture the sky
    //This script is used to pre-sample the screen color and use it to show what's behind the backface of the water, so that it can write to the depth buffer correctly
    public class AQUAS_CaptureBackground : MonoBehaviour
    {
        public GameObject waterObj;
        public RenderTexture target;
        Camera cam;

        public int quality = 2;

        public LayerMask layerMask = -1;

        //Set up target texture and camera
        void Start()
        {
            int i = (int)Mathf.Pow(2, quality);
            target = new RenderTexture(Screen.width / i, Screen.height / i, 32, RenderTextureFormat.ARGBHalf);

            cam = GetComponent<Camera>();
            cam.targetTexture = target;
            cam.pixelRect = new Rect(0, 0, Screen.width / i, Screen.height / i);
        }

        //Make sure the water is not captured by moving it to a layer that is ignored by the camera
        private void OnPreCull()
        {
            if (waterObj == null)
            {
                return;
            }
            cam.cullingMask = layerMask;
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Water"));
            waterObj.layer = LayerMask.NameToLayer("Water");
        }

        //Make sure to move the water back to the default layer, when done
        private void OnPostRender()
        {
            if (waterObj == null)
            {
                return;
            }
            waterObj.layer = LayerMask.NameToLayer("Default");
        }


        //Auto-adjust the size of the target texture on runtime
        //Unstable - Sometimes it will work nicely, sometimes it will crash Unity for no obvious reason, thus outcommented
        void Update()
        {
            if (target.width != Screen.width || target.height != Screen.height)
            {
                target.Release();
                target.width = Screen.width;
                target.height = Screen.height;
            }
        }

        //Simply render the scene to the texture without any processing of the result
        //Pass the result to the parameter of the backface material
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination);

            if (waterObj != null)
            {
                waterObj.GetComponent<Renderer>().sharedMaterials[1].SetTexture("_BackgroundTex", target);

                target.Release();

                return;
            }

            target.Release();
        }
    }
}
