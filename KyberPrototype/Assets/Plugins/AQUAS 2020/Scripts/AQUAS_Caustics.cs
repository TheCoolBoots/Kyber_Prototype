using UnityEngine;

namespace AQUAS
{
    //[ExecuteInEditMode]
    public class AQUAS_Caustics : MonoBehaviour
    {

        #region Variables
        public float fps;
        public Texture2D[] frames;
        public float maxCausticDepth;

        int frameIndex;
        Projector projector;

        float currentScale;
        float aspectRatio;

        //Hide because it shouldn't be manually altered, but must be accessed by the projected grid script, if it exists
        [HideInInspector]
        public float waterLevel;
        [HideInInspector]
        public bool overrideWaterLevel = false;

        #endregion

        //Initialize caustic image sequence
        void Start()
        {
            projector = GetComponent<Projector>();
            NextFrame();
            InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
            projector.material.SetFloat("_WaterLevel", transform.parent.transform.position.y);
            projector.material.SetFloat("_DepthFade", transform.parent.transform.position.y - maxCausticDepth);

            currentScale = transform.parent.localScale.z;
            aspectRatio = transform.parent.localScale.x / transform.parent.localScale.z;
            waterLevel = transform.parent.position.y;
        }

        //<summary>
        //Adjusts the max caustic depth
        //</summary>
        void Update()
        {
            if (overrideWaterLevel && transform.parent == null)
            {
                projector.material.SetFloat("_DepthFade", waterLevel - maxCausticDepth);
            }
            else
            {
                if (transform.parent == null)
                {
                    return;
                }

                projector.material.SetFloat("_DepthFade", transform.parent.transform.position.y - maxCausticDepth);
            }


//#if UNITY_EDITOR
            if (overrideWaterLevel)
            {
                currentScale = 1000;
                GetComponent<Projector>().orthographicSize = currentScale;
            }

            else
            {
                if(transform.parent == null)
                {
                    return;
                }
                currentScale = Mathf.Max(transform.parent.localScale.x, transform.parent.localScale.z) * 5;

                if (currentScale != transform.parent.localScale.z)
                {
                    GetComponent<Projector>().orthographicSize = currentScale;
                }
                if (aspectRatio != transform.parent.localScale.x / transform.parent.localScale.z)
                {
                    aspectRatio = transform.parent.localScale.x / transform.parent.localScale.z;
                    GetComponent<Projector>().aspectRatio = aspectRatio;
                }
            }

            if (overrideWaterLevel)
            {
                projector.material.SetFloat("_WaterLevel", waterLevel);
            }
            else
            {
                if (waterLevel != transform.parent.position.y)
                {
                    waterLevel = transform.parent.position.y;
                    projector.material.SetFloat("_WaterLevel", waterLevel);
                }
            }

            
//#endif
        }

        //<summary>
        //Set current caustic texture based on fps
        //</summary>
        void NextFrame()
        {
            projector.material.SetTexture("_Texture", frames[frameIndex]);
            frameIndex = (frameIndex + 1) % frames.Length;
        }

    }
}
