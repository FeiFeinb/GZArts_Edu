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
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //框架可行，bool换成integer
        if (PortConnectController.Instance.DigitalSignal)
        {
            animator.SetBool("Start", true);
            animator.SetFloat("PlayerSpeed", forwardSpeed);
            if (stateInfo.IsName("Flower") && stateInfo.normalizedTime >= 1)
            {
                // 播放完毕
            }
        }
        else
        {
            animator.SetFloat("PlayerSpeed", reverseSpeed);
            if (stateInfo.normalizedTime <= 0 && stateInfo.IsName("Flower"))
            {
                animator.SetBool("Start", false);
            }
        }
    }
}
