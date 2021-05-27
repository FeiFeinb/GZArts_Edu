using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SitImageController : MonoBehaviour
{
    private Animator _animator;
    private bool lastIsSit = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isSit = PortConnectController.Instance.TotalSignal;
        if (lastIsSit == false && isSit == true)
        {
            _animator.SetTrigger("Sit");
        }
        lastIsSit = isSit;
    }
}
