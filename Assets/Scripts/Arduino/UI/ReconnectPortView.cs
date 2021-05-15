using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ReconnectPortView : BaseUIView, IInit
{
    public static ReconnectPortView controller;
    
    [SerializeField] private Button _cancleButton;      // 取消按钮
    [SerializeField] private Text reconnectText;        // 重新连接文字UI
    
    private Coroutine currentCoroutine;                 // 重新连接协程
    
    public void Init()
    {
        _cancleButton.onClick.AddListener(CancleReconnect);
    }
    
    /// <summary>
    /// 开始重连
    /// </summary>
    public void Reconnect()
    {
        // 开启UI界面
        Show();
        // 启动协程
        if (currentCoroutine != null)
        {
            StopCoroutine(TryReconnectLoop());
        }
        currentCoroutine = StartCoroutine(TryReconnectLoop());
    }
    
    /// <summary>
    /// 手动取消重连
    /// </summary>
    private void CancleReconnect()
    {
        // 关闭协程
        if (currentCoroutine != null)
        {
            StopCoroutine(TryReconnectLoop());
        } 
        // 关闭UI界面
        Hide();
    }


    private IEnumerator TryReconnectLoop()
    {
        void SetTextAnimation()
        {
            string str = "正在重连";
            for (int i = 0; i <= (int) Time.time % 3; i++)
            {
                str = string.Concat(str, ".");
            }
            reconnectText.text = str;
        }
        
        while (true)
        {
            // 每一秒请求一次重连
            yield return new WaitForSeconds(1);
            // 设置等待动画
            SetTextAnimation();
            // 尝试重连
            PortConnectController.Instance.TryReconnect();
            // 判断重连是否成功
            if (PortConnectController.Instance.IsConnected)
            {
                Debug.Log("重新连接成功");
                break;
            }
        }
        // 重连成功
        currentCoroutine = null;
        Hide();
    }

    
}
