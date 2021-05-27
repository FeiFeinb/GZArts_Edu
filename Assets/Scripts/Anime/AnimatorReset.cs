using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AnimatorReset : BaseSingletonWithMono<AnimatorReset>
{
    [SerializeField] private RuntimeAnimatorController[] _animeControllerA;
    [SerializeField] private RuntimeAnimatorController[] _animeControllerB;
    [SerializeField] private RuntimeAnimatorController[] _animeControllerC;


    [SerializeField] private Animator _animatorA;
    [SerializeField] private Animator _animatorB;
    [SerializeField] private Animator _animatorC;

    [SerializeField] private int playTimes = 0;

    private float _timer;
    private void Start()
    {
        playTimes = 0;
    }

    private void AnimatorSwitch()
    {
        playTimes++;
        _animatorA.runtimeAnimatorController = _animeControllerA[playTimes % 3];
        _animatorB.runtimeAnimatorController = _animeControllerB[playTimes % 3];
        _animatorC.runtimeAnimatorController = _animeControllerC[playTimes % 3];
    }
    public void Reset()
    {
        if (_timer > 1.0f)
        {
            AnimatorSwitch();
            _animatorA.SetTrigger("Reset");
            _animatorB.SetTrigger("Reset");
            _animatorC.SetTrigger("Reset");
            _timer = 0;
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }
}
