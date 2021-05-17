using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //框架可行，bool换成integer
        if (PortConnectController.Instance.DigitalSingnal)
        {
            animator.SetBool("Start", true);
            animator.SetFloat("PlayerSpeed", 8);
            if (stateInfo.IsName("Flower") && stateInfo.normalizedTime >= 1)
            {
                animator.SetBool("Over", true);
            }
        }
        else
        {
            animator.SetBool("Over", false);
            animator.SetFloat("PlayerSpeed", -1);
            if (stateInfo.normalizedTime <= 0 && stateInfo.IsName("Flower"))
            {
                animator.SetBool("Start", false);
            }
        }
    }
}
