using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator), typeof(AnimationControllerManager))]
public class ImageController : MonoBehaviour
{

    [SerializeField, Tooltip("正向播放速度")] private float _forwardSpeed = 1;
    [SerializeField, Tooltip("反向播放速度")] private float _reverseSpeed = -1;

    // [SerializeField, Tooltip("持续多长秒无人坐下动画开始回退")]
    // private float _reverseTime = 3.0f;

    private Animator animator; // 图片动画机

    [SerializeField] private float reverseTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isTrigger = PortConnectController.Instance.TotalSignal;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("Sit", isTrigger);
        animator.SetBool("Reverse", !isTrigger);
        animator.SetFloat("PlaySpeed", isTrigger ? _forwardSpeed : _reverseSpeed);
        // 有人坐下 取消回退
        // if (isTrigger == true)
        // {
        //     animator.SetBool("Reverse", false);
        // }
        // 判断动画是否处于循环状态 如果是则开始回退时间计时
        // AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // if (stateInfo.IsTag("Loop"))
        // {
        //     reverseTimer += Time.deltaTime;
        //     if (reverseTimer >= _reverseTime)
        //     {
        //         animator.SetBool("Reverse", true);
        //         reverseTimer = 0;
        //     }
        // }
        // else
        // {
        //     reverseTimer = 0;
        // }
    }

    //动画事件
    private void ChangeAnimator()
    {
        AnimatorReset.Instance.Reset();
    }
}