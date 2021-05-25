using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInput : BaseSingletonWithMono<PlayerInput>
{
    [SerializeField] private MessageDebuggerView messageViewDebuggerA;
    [SerializeField] private MessageDebuggerView messageViewDebuggerB;
    [SerializeField] private MessageDebuggerView messageViewDebuggerC;
    private void Update()
    {
        // 打开串口选择界面
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PortsSelectorView.controller.isActive)
            {
                PortsSelectorView.controller.Hide();
            }
            else
            {
                PortsSelectorView.controller.Show();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (messageViewDebuggerA.isActive)
            {
                messageViewDebuggerA.Hide();
                messageViewDebuggerB.Hide();
                messageViewDebuggerC.Hide();
            }
            else
            {
                messageViewDebuggerA.Show();
                messageViewDebuggerB.Show();
                messageViewDebuggerC.Show();
            }
        }

//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
// #if UNITY_EDITOR
//             EditorApplication.isPlaying = false;
// #else
//             Application.Quit();
// #endif
//         }
    }
}