using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AnimationControllerManager : MonoBehaviour
{ 
    [SerializeField] private RuntimeAnimatorController[] _animeController;

    [SerializeField] private ImageController _imageController;
    [SerializeField] private Animator _animator;

    [SerializeField] private int playTimes = 0;


    private void Start()
    {
        _imageController.changeAnimator += AnimatorSwitch;
        playTimes = 0;
    }

    private void AnimatorSwitch()
    {
        playTimes++;
        _animator.runtimeAnimatorController = _animeController[playTimes % 3];
    }
}