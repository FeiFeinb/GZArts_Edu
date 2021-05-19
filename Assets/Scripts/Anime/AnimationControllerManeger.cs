using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AnimationControllerManeger : MonoBehaviour
{
    [FormerlySerializedAs("animeController")] [SerializeField]
    private RuntimeAnimatorController[] _animeController;

    [SerializeField] private ImageController _imageController;
    [SerializeField] private Animator _animator;

    [SerializeField] private int playTimes = 0;

    [SerializeField] private List<int> playIndex = new List<int>() {0, 1, 2};

    private void Start()
    {
        _imageController.changeAnimator += ChangeAnimator;
        playTimes = 0;
        AnimeListControl();
    }

    private void ChangeAnimator()
    {
        _animator.runtimeAnimatorController = _animeController[playIndex[playTimes]];
        playTimes++;
        AnimeListControl();
        _imageController.WhetherDelay = false;
    }

    private void AnimeListControl()
    {
        if (playTimes < 3 && playTimes >= 1)
        {
            return;
        }

        playTimes = 0;
        //10次循环，降低重复率
        for (int i = 0; i < 10; i++)
        {
            int m = Random.Range(0, 3);
            int n = Random.Range(0, 3);

            int temp;
            temp = playIndex[m];
            playIndex[m] = playIndex[n];
            playIndex[n] = temp;
        }
    }
}