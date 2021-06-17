using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAppear : MonoBehaviour
{
    public Image image;
    void Start()
    {
        image.enabled = false;
    }

    public void makeImageAppear()
    {
        image.enabled = true;
    }

    public void makeImageDisappear()
    {
        image.enabled = false;
    }
}
