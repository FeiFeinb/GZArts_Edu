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
        // 显示串口输出
        EditorGUILayout.LabelField("数字信号1：", _controller.DigitalSignalOne ? "触发" : "不触发");
        EditorGUILayout.LabelField("数字信号2：", _controller.DigitalSignalTwo ? "触发" : "不触发");
        EditorGUILayout.LabelField("数字信号3：", _controller.DigitalSignalThree ? "触发" : "不触发");
        // 显示串口状态
        EditorGUILayout.LabelField("当前串口名称: ", string.IsNullOrEmpty(_controller.StashConnectPortName) ? "空" : _controller.StashConnectPortName);
        string stateStr = string.Concat(_controller.IsOpen ? "已打开" : "未打开", "     ", _controller.IsConnected ? "已连接" : "未连接");
        EditorGUILayout.LabelField("当前串口状态：", stateStr);
        // 强制重新绘制窗口 以刷新编辑器面板
        Repaint();
    }
}