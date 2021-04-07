using Outliner;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

namespace HoverOutliner
{
    public class OutlineController : MonoBehaviour
    {
        private Outline outline;
        void Start()
        {
            outline = GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.enabled = false;
        }

        void OnHandHoverBegin()
        {
            outline.enabled = true;
        }

        void OnHandHoverEnd()
        {
            outline.enabled = false;
        }

    }
}

