using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AQUAS
{
    [ExecuteInEditMode]

    //This script ensures the container holding the waterplanes can't be moved - it shouldn't really
    public class AQUAS_Container : MonoBehaviour
    {

        //This script only positions the AQUAS container at (0,0,0)
        //While in the editor, the container will be locked at that position
        void Start()
        {
            transform.position = new Vector3(0, 0, 0);
        }

        void Update()
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}