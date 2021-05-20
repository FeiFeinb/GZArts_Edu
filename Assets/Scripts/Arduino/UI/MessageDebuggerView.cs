using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
public class MessageDebuggerView : BaseUIView
{
    public static MessageDebuggerView controller;
    
    [SerializeField] private Text _aniamtorText;
    [SerializeField] private Text _animationText;
    [SerializeField] private Text _stateText;

    [SerializeField] private Animator _animator;
    private void Update()
    {
        var stateInfo = _animator.GetCurrentAnimatorClipInfo(0);
        
        _aniamtorText.text = string.Concat("当前动画机:", _animator.runtimeAnimatorController.name);
        _animationText.text = string.Concat("当前动画片:", stateInfo[0].clip.name);
        _stateText.text = string.Concat("当前人物状态:", PortConnectController.Instance.TotalSignal ? "人坐下" : "人起立");
    }
}
