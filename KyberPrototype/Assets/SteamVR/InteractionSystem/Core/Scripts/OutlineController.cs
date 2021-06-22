
using Valve.VR.InteractionSystem;
using UnityEngine;

namespace Kyber
{
    [RequireComponent(typeof(Outline))]
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