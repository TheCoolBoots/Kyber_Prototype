using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    private CharacterController controller;

    public void ToggleVisibilty()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();

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
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("XRI_Left_PrimaryTouch"))
            ToggleVisibilty();

    }
}
