using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _portsSelectorView;     // 串口选择UI
    [SerializeField] private GameObject _reconnectPortView;     // 重新连接UI
    [SerializeField] private GameObject _messageDebuggerView;   // 调试UI
    private void Start()
    {
        // UI初始化
        PortsSelectorView.controller = _portsSelectorView.GetComponent<PortsSelectorView>();
        PortsSelectorView.controller.PreInit();
        ReconnectPortView.controller = _reconnectPortView.GetComponent<ReconnectPortView>();
        ReconnectPortView.controller.PreInit();

        MessageDebuggerView.controller = _messageDebuggerView.GetComponent<MessageDebuggerView>();
    }
}
