using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Animator frontWheelAnimator;
    public float turnInput = 0;
    void Start()
    {
        frontWheelAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Mathf.Clamp(turnInput, -1, 1);
        frontWheelAnimator.SetFloat("TurnInput", turnInput);
    }
}
