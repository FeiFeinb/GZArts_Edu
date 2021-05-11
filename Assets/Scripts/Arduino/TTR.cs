using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.Threading;

public class TTR : MonoBehaviour
{
    private SerialPort port = null;
    private Thread thread;
    private bool isConnected;
    private bool select;
    private string Com;

    [Header("串口")]
    public string[] Text;
    public string digital, analog;
    public string[] ports;
    public Dropdown dropdown;

    [Header("动画相关")]
    public int state_0;
    public int state_1, state_2;
    public Animator animator;
    public int RandomPlay;
    public bool isOriginalState, StatePlay;
    //private AnimatorClipInfo clipInfo;
    private AnimatorStateInfo stateInfo;
    public bool Initial, state, blossom, fuulblossom;
    public float InvertedSpeed;

    //测试多字节传输
    [HideInInspector]
    public byte[] receive = new byte[16];

    private void Start()
    {
        //stateInfo = GetComponent<AnimatorStateInfo>();
        ports = GetPorts();
        thread = new Thread(MessageThread);
        //port = new SerialPort(Com,9600);
        //port.Open();
        thread.Start();

        //StartCoroutine("enumerator");
    }

    #region //测试串口传输
    //IEnumerator enumerator()
    //{
    //    while (true)
    //    {
    //        Text = port.ReadLine();
    //        port.DiscardInBuffer();
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}
    #endregion

    private void FixedUpdate()
    {
        DropDownControl();
        Function();
        AnimControl();
    }

    //基本功能
    private void Function()
    {
        if (!isConnected)
        {
            ReConnectCom();
            return;
        }

        if (animator == null)
        {
            Debug.LogWarning("animator为空，需要添加");
            return;
        }

        if (port.PortName != ports[ports.Length - 1])
        {
            isConnected = false;
            return;
        }
        else
        {
            if (analog != "0")
            {

                animator.SetFloat("Inverted", InvertedSpeed);

            }
            else
            {
                animator.SetFloat("Inverted", 1);
            }
        }
    }

    //串口重连
    private void ReConnectCom()
    {
        if (!select)
            return;

        if (port.PortName != ports[ports.Length - 1])
        {
            return;
        }
        try
        {
            if (port.IsOpen)
            {
                port.Close();
            }
            else
            {
                //4个初始串口均为关闭
                port.Open();
                Debug.Log("Connect" + ports[ports.Length - 1]);
                isConnected = true;
            }

        }
        catch
        {
            Debug.LogError("CantConnectCom2");
        }
    }

    //显示串口
    private string[] GetPorts()
    {
        string[] portnames = SerialPort.GetPortNames();
        return portnames;
    }

    //另开线程传递消息
    private void MessageThread()
    {
        while (true)
        {
            ports = GetPorts();
            if (isConnected)
                try
                {
                    Text = port.ReadLine().Split(',');
                    digital = Text[0];
                    analog = Text[1];

                }
                catch
                {

                }

        }
    }
   
    // 串口选择
    private void DropDownControl()
    {
        if (dropdown == null)
        {
            Debug.LogWarning("需要添加UI/DROPDOWN组件");
            return;
        }

        if (isConnected)
        {
            if (dropdown.gameObject.activeSelf == true)
                dropdown.gameObject.SetActive(false);
            return;
        }

        dropdown.options.Clear();
        Dropdown.OptionData temoData;
        temoData = new Dropdown.OptionData();
        dropdown.options.Add(temoData);
        for (int i = 1; i < ports.Length + 1; i++)
        {
            //给每一个option选项赋值
            temoData = new Dropdown.OptionData();
            temoData.text = ports[i - 1];
            dropdown.options.Add(temoData);
        }

        if(dropdown.value != 0)
        {
            Com = dropdown.captionText.text;
            Addport();   
        }
    }

    //添加串口
    private void Addport()
    {
        if (!select)
        {
            port = new SerialPort(Com, 9600);
            select = true;
        }
    }

    //动画切换
    private void AnimControl()
    {
        if (animator == null)
            return;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //获取当前动画名称
        if(stateInfo.IsName("State"))
        {
            isOriginalState = true;
        }

        //判断动画播放
        if(StatePlay && digital.Equals("1"))
        {
            RandomPlay = Random.Range(1, 5);
            if (isOriginalState)
            {
                animator.SetInteger("State", RandomPlay);
                isOriginalState = false;
            }
            StatePlay = false;
        }
        else if(digital.Equals("0"))
        {
            animator.SetInteger("State", 0);
        }
        
        //动画可播放进程

    }

    //关闭串口。线程
    private void OnApplicationQuit()
    {
        thread.Abort();
        if(isConnected)
        port.Close();
    }
}
