using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(AnimationControllerManeger))]
public class ImageController : MonoBehaviour
{
    [SerializeField, Tooltip("正向播放速度")] private float forwardSpeed = 1;
    [SerializeField, Tooltip("反向播放速度")] private float reverseSpeed = -1;
    [SerializeField, Tooltip("动画是否延迟")] private bool WhetherDelay = false;
    [SerializeField,Tooltip("延迟时间")] private float Delaytime;

    private Animator animator;
    private AnimatorStateInfo stateInfo;


    private float t = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("WhetherDelay", WhetherDelay);

        PlayDelay();

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

    //播放延迟
    private void PlayDelay()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("待机动画") && WhetherDelay && PortConnectController.Instance.DigitalSignal)
        {
            t += Time.deltaTime;
            if(t >= Delaytime)
            {
                t = 0;
                WhetherDelay = false;
            }
        }
        else if(!PortConnectController.Instance.DigitalSignal)
        {
            WhetherDelay = false;
        }
    }

    //动画事件
    private void AnimeStartSetTrue()
    {
        WhetherDelay = false;
    }

    //动画事件
    private void AnimeStartSetFalse()
    {
        WhetherDelay = true;
    }
}
