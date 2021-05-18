using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ImageController : MonoBehaviour
{
    [SerializeField, Tooltip("正向播放速度")] private float forwardSpeed = 1;
    [SerializeField, Tooltip("反向播放速度")] private float reverseSpeed = -1;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PortConnectController.Instance.DigitalSignal)
        {
            animator.SetBool("Start", true);
            animator.SetFloat("PlaySpeed", forwardSpeed);
        }
        else
        {
            animator.SetFloat("PlaySpeed", reverseSpeed);
            animator.SetBool("Start", false);
        }
        animator.SetBool("Sit", PortConnectController.Instance.DigitalSignal);
    }
}
