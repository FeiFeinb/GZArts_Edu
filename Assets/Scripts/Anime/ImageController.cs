using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator), typeof(AnimationControllerManeger))]
public class ImageController : MonoBehaviour
{
    public bool WhetherDelay
    {
        get
        {
            return _whetherDelay;
        }
        set
        {
            _whetherDelay = value;
        }
    }

    public Action changeAnimator;
    
    [SerializeField, Tooltip("正向播放速度")] private float _forwardSpeed = 1;
    [SerializeField, Tooltip("反向播放速度")] private float _reverseSpeed = -1;
    [SerializeField, Tooltip("动画是否延迟")] private bool _whetherDelay = false;
    [SerializeField,Tooltip("延迟时间")] private float _delaytime;

    private Animator animator;
    private AnimatorStateInfo stateInfo;


    private float t = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayDelay();
        animator.SetBool("WhetherDelay", _whetherDelay);
        if (PortConnectController.Instance.TotalSignal)
        {
            animator.SetBool("Start", true);
            animator.SetFloat("PlaySpeed", _forwardSpeed);
        }
        else
        {
            animator.SetFloat("PlaySpeed", _reverseSpeed);
            animator.SetBool("Start", false);
        }
        animator.SetBool("Sit", PortConnectController.Instance.TotalSignal);
    }

    //播放延迟
    private void PlayDelay()
    {
        // stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(_whetherDelay && PortConnectController.Instance.TotalSignal)
        {
            t += Time.deltaTime;
            if(t >= _delaytime)
            {
                t = 0;
                _whetherDelay = false;
            }
        }
        else if(!PortConnectController.Instance.TotalSignal)
        {
            _whetherDelay = false;
        }
    }

    //动画事件
    private void AnimeStartSetTrue()
    {
        changeAnimator?.Invoke();
        _whetherDelay = true;
        animator.SetBool("WhetherDelay", _whetherDelay);
    }

    //动画事件
    private void AnimeStartSetFalse()
    {
        _whetherDelay = false;
    }
}
