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

    [Header("����")]
    public string[] Text;
    public string digital, analog;
    public string[] ports;
    public Dropdown dropdown;

    [Header("�������")]
    public int state_0;
    public int state_1, state_2;
    public Animator animator;
    public int RandomPlay;
    public bool isOriginalState, StatePlay;
    //private AnimatorClipInfo clipInfo;
    private AnimatorStateInfo stateInfo;
    public bool Initial, state, blossom, fuulblossom;
    public float InvertedSpeed;

    //���Զ��ֽڴ���
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

    #region //���Դ��ڴ���
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

    //��������
    private void Function()
    {
        if (!isConnected)
        {
            ReConnectCom();
            return;
        }

        if (animator == null)
        {
            Debug.LogWarning("animatorΪ�գ���Ҫ���");
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

    //��������
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
                //4����ʼ���ھ�Ϊ�ر�
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

    //��ʾ����
    private string[] GetPorts()
    {
        string[] portnames = SerialPort.GetPortNames();
        return portnames;
    }

    //���̴߳�����Ϣ
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
   
    // ����ѡ��
    private void DropDownControl()
    {
        if (dropdown == null)
        {
            Debug.LogWarning("��Ҫ���UI/DROPDOWN���");
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
            //��ÿһ��optionѡ�ֵ
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

    //��Ӵ���
    private void Addport()
    {
        if (!select)
        {
            port = new SerialPort(Com, 9600);
            select = true;
        }
    }

    //�����л�
    private void AnimControl()
    {
        if (animator == null)
            return;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //��ȡ��ǰ��������
        if(stateInfo.IsName("State"))
        {
            isOriginalState = true;
        }

        //�ж϶�������
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
        
        //�����ɲ��Ž���

    }

    //�رմ��ڡ��߳�
    private void OnApplicationQuit()
    {
        thread.Abort();
        if(isConnected)
        port.Close();
    }
}
