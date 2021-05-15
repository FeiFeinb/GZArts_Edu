using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _portsSelectorView;     // 串口选择UI
    [SerializeField] private GameObject _reconnectPortView;     // 重新连接UI
    private void Start()
    {
        // UI初始化
        PortsSelectorView.controller = _portsSelectorView.GetComponent<PortsSelectorView>();
        PortsSelectorView.controller.Init();
        ReconnectPortView.controller = _reconnectPortView.GetComponent<ReconnectPortView>();
        PortsSelectorView.controller.Init();
    }
}
