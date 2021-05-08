using UnityEngine;
using System.Collections;

namespace AQUAS
{

    //Use this to manipulate the render queue to AQUAS's shaders if you need to (can be done from the material inspector since Unity 2017)
    [ExecuteInEditMode]
    [AddComponentMenu("AQUAS/Utils/Render Queue Controller")]
    public class AQUAS_RenderQueueEditor : MonoBehaviour
    {
        public int renderQueueIndex = -1;
        void Update()
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.renderQueue = renderQueueIndex;
        }
    }
}

