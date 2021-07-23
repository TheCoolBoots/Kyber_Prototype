using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Objectives : MonoBehaviour
{
    private Renderer rend;
    
    public SteamVR_Action_Boolean xbutton;

    public void ToggleVisibilty()
    {
        if(rend.enabled)
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(xbutton.stateDown)
        {
            Debug.Log("Player pressed x down");
            ToggleVisibilty();
        }
    }
}
