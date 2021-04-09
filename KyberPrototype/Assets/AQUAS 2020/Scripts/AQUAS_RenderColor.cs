using UnityEngine;

namespace AQUAS
{
    //This script renders a screen buffer to a render texture
    //The buffer can later be used to bypass a step in Unity's render pipeline
    public class AQUAS_RenderColor : MonoBehaviour
    {
        public float transparencyQuality = 0;

        [HideInInspector]
        public GameObject plane;

        RenderTexture target;

        Camera cam;
        public LayerMask layerMask = -1;

        // Use this for initialization
        void Start()
        {
            float i = Mathf.Pow(2f, transparencyQuality);
            target = new RenderTexture(Screen.width/(int)i, Screen.height/(int)i, 32, RenderTextureFormat.ARGBHalf);
            target.filterMode = FilterMode.Trilinear;

            cam = GetComponent<Camera>();
            cam.targetTexture = target;

        }

        private void OnPreCull()
        {
            if (plane == null)
            {
                return;

            }

            cam.cullingMask = layerMask;
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Water"));
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
            Graphics.Blit(source, destination);

            if (plane != null)
            {
                plane.GetComponent<Renderer>().sharedMaterials[0].SetTexture("_ColorTex", target);

                target.Release();

                return;
            }

            target.Release();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
