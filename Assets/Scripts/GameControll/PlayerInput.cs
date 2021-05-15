using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : BaseSingletonWithMono<PlayerInput>
{
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
    }
}
