using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using System.Windows;
using UnityEditor;
using UnityEngine.PlayerLoop;

public class PortConnectController : BaseSingletonWithMono<PortConnectController>
{
    public string StashConnectPortName => _stashConnectPortName;
    public bool DigitalSignalOne => _digitalSignalOne == "1" ? true : false;
    public bool DigitalSignalTwo => _digitalSignalTwo == "1" ? true : false;
    public bool DigitalSignalThree => _digitalSignalThree == "1" ? true : false;
    public string DigitalSignalOneStr => _digitalSignalOne;
    public string DigitalSignalTwoStr => _digitalSignalTwo;
    public string DigitalSignalThreeStr => _digitalSignalThree;
        public bool IsConnected => isConnected;

    public bool IsOpen
    {
        get
        {
            // 串口不为空且已打开
            return _port != null && _port.IsOpen;
        }
    }
    
    private bool isConnected;               // 是否已连接单片机
    private bool isInitiativeDisConnect;    // 是否主动断开连接
    private string _stashConnectPortName;   // 所连接的串口名字
    
    private string _digitalSignalOne;       // 串口输出数字信号1
    private string _digitalSignalTwo;       // 串口输出数字信号2
    private string _digitalSignalThree;     // 串口输出数字信号3
    
    private SerialPort _port;               // 当前连接串口
    private Thread _messageHandleThread;    // 处理串口消息线程

    private bool _reconnectedTrigger;       // 用于子线程向主线程发送消息
    
    private void Start()
    {
        // 开启线程
        _messageHandleThread = new Thread(HandleMessage);
        _messageHandleThread.Start();
    }

    private void Update()
    {
        // 需要重连且非玩家主动断开连接
        if (_reconnectedTrigger)
        {
            // 重新连接
            ReconnectPortView.controller.Reconnect();
            _reconnectedTrigger = false;
        }
    }

    private void OnApplicationQuit()
    {
        // 应用结束后关闭线程
        _messageHandleThread.Abort();
        // 断开串口连接
        DisConnect();
    }
    
    /// <summary>
    /// 连接串口
    /// </summary>
    /// <param name="portName">串口名称</param>
    /// <returns>连接是否成功</returns>
    public void Connect(string portName)
    {
        try
        {
            if (isConnected)
            {
                // 如果重复连接同一个串口 则返回
                if (_port.PortName == portName) return;
                // 连接其他串口
                DisConnect();
            }
            // 连接
            _port = new SerialPort(portName, 9600);
            _port.Open();
            if (IsOpen)
            {
                // 设置串口名称
                _stashConnectPortName = _port.PortName;
                // 设置为已连接
                isConnected = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log("未找到相应的串口可供连接");
        }
    }

    /// <summary>
    /// 断开串口
    /// </summary>
    public void DisConnect()
    {
        // 若串口打开则同时关闭串口
        if (IsOpen)
        {
            _port.Close();
            _port = null;
            isConnected = false;
        }
    }

    /// <summary>
    /// 玩家主动断开连接
    /// </summary>
    public void InitiativeDisconnect()
    {
        isInitiativeDisConnect = true;
        DisConnect();
    }

    public void InitiativeConnect(string portName)
    {
        isInitiativeDisConnect = false;
        Connect(portName);
    }
    /// <summary>
    /// 尝试重新连接串口
    /// </summary>
    public void TryReconnect()
    {
        DisConnect();
        Connect(_stashConnectPortName);
    }

    /// <summary>
    /// 用于线程处理单片机串口输出
    /// </summary>
    private void HandleMessage()
    {
        while (true)
        {
            if (IsOpen && isConnected)
            {
                try
                {
                    string[] message = _port.ReadLine().Split(',');
                    _digitalSignalOne = message[0];
                    _digitalSignalTwo = message[1];
                    _digitalSignalThree = message[2];
                }
                catch (Exception e)
                {
                    isConnected = false;
                    // 判断是否需要重连
                    _reconnectedTrigger = !isInitiativeDisConnect;
                }
            }
        }
    }
}