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
        // 检测新取得的串口数组时候与旧的相同
        List<string> tempPortsName = SerialPort.GetPortNames().ToList();
        // 相同则跳过
        if (IsEqual(tempPortsName)) return;
        // 不同则更改
        _portsName = tempPortsName;
        onPortsChange?.Invoke(_portsName);
    }

    private bool IsEqual(List<string> tempPortsName)
    {
        // 检测两个数组是否相等
        if (tempPortsName.Count == _portsName.Count)
        {
            for (int i = 0; i < tempPortsName.Count; i++)
            {
                if (tempPortsName[i] != _portsName[i])
                    return false;
            }
            return true;
        }
        return false;
    }
}
