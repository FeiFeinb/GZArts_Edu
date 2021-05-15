using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;
/// <summary>
/// 管理计算机中的串口
/// </summary>
public class PortsManager : BaseSingletonWithMono<PortsManager>
{
    [SerializeField] private List<string> _portsName = new List<string>();     // 计算机现有串口名称数列

    public Action<IEnumerable<string>> onPortsChange;                          // 串口数列更新事件
    private void Update()
    {
        string[] tempPortsName = SerialPort.GetPortNames();
        if (tempPortsName.Length == _portsName.Count)
        {
            for (int i = 0; i < tempPortsName.Length; i++)
            {
                if (tempPortsName[i] == _portsName[i])
                    continue;
            }
        }
        _portsName = tempPortsName.ToList();
        onPortsChange?.Invoke(_portsName);
    }
}
