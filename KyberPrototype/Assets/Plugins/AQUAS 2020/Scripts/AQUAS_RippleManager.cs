using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AQUAS
{
    [AddComponentMenu("AQUAS/Essentials/Ripple Effect")]
    public class AQUAS_RippleManager : MonoBehaviour
    {
        [Range(0, 4)]
        public int numberOfObjects = 0;

        [Space(10)]
        [TextArea(1, 3)]
        public string objectsInfo = "Array of objects to produce ripples (max. 4). Changing the number of objects will clear the array!";
        [Space(10)]

        public GameObject[] objects;

        [Space(10)]
        [Header("Waterplane to apply ripples to.")]
        [Space(10)]

        public GameObject waterplane;
        
        private void OnValidate()
        {
            objectsInfo = "Array of objects to produce ripples (max. 4). Changing the number of objects will clear the array!";
            
            //Make sure the array never has more than 4 elements!
            if (objects.Length != numberOfObjects)
            {
                objects = new GameObject[numberOfObjects];
            }
        }
        
        void Start()
        {
            //For every object...
            for (int i = 0; i < objects.Length; i++)
            {
                //Check if array element holds a game object
                if (objects[i] == null)
                {
                    Debug.LogWarning("Gameobject in array is null. Skipping...");
                    continue;
                }

                //Check if object has a mesh
                if (objects[i].GetComponent<MeshFilter>() == null)
                {
                    Debug.LogWarning("Object " + objects[i].name + " does not have a Mesh Filter component. Skipping...");
                    continue;
                }

                //...instantiate a ripple recorder
                GameObject rippleRecorder = Instantiate(Resources.Load("RippleRecorder", typeof(GameObject))) as GameObject;
                rippleRecorder.name = "RippleRecorder" + i.ToString();

                //...set neccessary parameters in the RippleController component
                AQUAS_RippleController rippleController = rippleRecorder.GetComponent<AQUAS_RippleController>();
                rippleController.body = objects[i];
                rippleController.waterPlane = waterplane;
                rippleController.index = i;

                //...and make sure the camera renders early.
                rippleRecorder.GetComponent<Camera>().depth = -1000 - i;
            }
        }
    }
}
