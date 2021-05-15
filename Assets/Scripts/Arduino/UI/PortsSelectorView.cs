using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class PortsSelectorView : BaseUIView, IInit
{
    public static PortsSelectorView controller;
    
    [SerializeField] private Dropdown _selector;        // 串口选择UI
    [SerializeField] private Button _quitButton;        // 退出按钮
    
    public void Init()
    {
        // 添加选择监听
        _selector.onValueChanged.AddListener(OnSelectPort);
        _quitButton.onClick.AddListener(Quit);
        // 添加刷新DropDownUI监听
        PortsManager.Instance.onPortsChange += GenerateDropDown;
    }

    private void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ResetDropDown()
    {
        _selector.value = 0;
    }
    /// <summary>
    /// 选择DropDownUI回调
    /// </summary>
    /// <param name="index">索引</param>
    private void OnSelectPort(int index)
    {
        // 断开连接
        if (index == 0)
        {
            PortConnectController.Instance.InitiativeDisconnect();
        }
        else
        { 
            PortConnectController.Instance.InitiativeConnect(_selector.captionText.text);
        }

        _selector.captionText.text = _selector.options[index].text;
    }
    
    /// <summary>
    /// 根据串口名称列表生成DropDown
    /// </summary>
    /// <param name="portsName">串口名称列表</param>
    private void GenerateDropDown(IEnumerable<string> portsName)
    {
        // 重新建立前清空
        _selector.options.Clear();
        // 创建空选项
        Dropdown.OptionData emptyDropDown = new Dropdown.OptionData();
        emptyDropDown.text = "不选择";
        _selector.options.Add(emptyDropDown);
        // 创建Ports选择列表
        foreach (string portName in portsName)
        {
            Dropdown.OptionData portDropDown = new Dropdown.OptionData();
            portDropDown.text = portName;
            _selector.options.Add(portDropDown);
        }
        int index = (int)Mathf.Lerp(0f, (float)_selector.options.Count - 1, (float)_selector.value);
        _selector.captionText.text = _selector.options[index].text;
    }
}
