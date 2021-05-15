using System;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(PortConnectController))]
public class PortConnectControllerEditor : Editor
{
    private PortConnectController _controller;
    private void OnEnable()
    {
        _controller = target as PortConnectController;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        // 显示串口输出
        EditorGUILayout.LabelField("数字信号：", string.IsNullOrEmpty(_controller.DigitalSingnal) ? "无" : _controller.DigitalSingnal);
        EditorGUILayout.LabelField("模拟信号：", string.IsNullOrEmpty(_controller.AnalogSignal) ? "无" : _controller.AnalogSignal);
        // 显示串口状态
        EditorGUILayout.LabelField("当前串口名称: ", string.IsNullOrEmpty(_controller.StashConnectPortName) ? "空" : _controller.StashConnectPortName);
        string stateStr = string.Concat(
            _controller.IsOpen ? "已打开" : "未打开", "     ", 
            _controller.IsConnected ? "已连接" : "未连接");
        EditorGUILayout.LabelField("当前串口状态：", stateStr);
    }
}