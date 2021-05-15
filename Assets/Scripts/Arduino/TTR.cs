using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.Threading;

public class TTR : MonoBehaviour
{
    private static TTR instance = null;

    public static TTR Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("串口")]
    public bool isConnected;
    public string[] Text;
    public string digital, analog;
    public string[] ports;
    public Dropdown dropdown;
    private SerialPort port = null;
    private Thread thread;
    private bool select;
    private string Com;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dropdown.onValueChanged.AddListener(DropDownChange);
        thread = new Thread(MessageThread);
        thread.Start();
    }

    private void FixedUpdate()
    {
        DropDownControl();
        DropDownActive();
        Function();
    }

    //基本功能
    private void Function()
    {
        ports = GetPorts();

        if (!isConnected)
        {
            ReConnectCom();
            return;
        }
    }

    //串口重连
    private void ReConnectCom()
    {
        if (!select)
            return;

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
                Debug.Log("Connect" + dropdown.captionText.text);
                isConnected = true;
            }

        }
        catch
        {
            Debug.LogError("CantConnectCom");
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

        dropdown.options.Clear();
        Dropdown.OptionData temoData;
        temoData = new Dropdown.OptionData();
        dropdown.options.Add(temoData);
        for (int i = 0; i < ports.Length; i++)
        {
            //给每一个option选项赋值
            temoData = new Dropdown.OptionData();
            temoData.text = ports[i];
            dropdown.options.Add(temoData);
        }
    }

    //串口改变并添加串口
    private void DropDownChange(int value)
    {
        select = value == 0 ? false : true;

        if (port != null)
        {
            port.Close();
            port = null;
        }

        if (select)
        {
            Com = dropdown.captionText.text;
            port = new SerialPort(Com, 9600);
            isConnected = false;
            dropdown.gameObject.SetActive(false);
        }
    }

    //dropdown显示
    private void DropDownActive()
    {
        if (dropdown.gameObject.activeSelf == false)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
                if (Input.GetKeyDown(KeyCode.Q))
                    dropdown.gameObject.SetActive(true);
        }
    }

    //关闭串口、线程
    private void OnApplicationQuit()
    {
        thread.Abort();
        if (isConnected)
            port.Close();
    }
}
