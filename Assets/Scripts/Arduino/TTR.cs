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
    private AnimatorStateInfo stateInfo;
    public bool Initial, state, blossom, fuulblossom;
    public float InvertedSpeed;

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
        AnimControl();
        Function();
    }

    //��������
    private void Function()
    {
        ports = GetPorts();

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

        if (analog != "0")
        {

            animator.SetFloat("Inverted", InvertedSpeed);

        }
        else
        {
            animator.SetFloat("Inverted", 1);
        }

    }

    //��������
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
                //4����ʼ���ھ�Ϊ�ر�
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

        dropdown.options.Clear();
        Dropdown.OptionData temoData;
        temoData = new Dropdown.OptionData();
        dropdown.options.Add(temoData);
        for (int i = 0; i < ports.Length; i++)
        {
            //��ÿһ��optionѡ�ֵ
            temoData = new Dropdown.OptionData();
            temoData.text = ports[i];
            dropdown.options.Add(temoData);
        }
    }

    //���ڸı䲢��Ӵ���
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

    //dropdown����
    private void DropDownActive()
    {
        if (dropdown.gameObject.activeSelf == false)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
                if (Input.GetKeyDown(KeyCode.Z))
                    dropdown.gameObject.SetActive(true);
        }
    }

    //�����л�
    private void AnimControl()
    {
        if (animator == null)
            return;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //��ȡ��ǰ��������
        if (stateInfo.IsName("State"))
        {
            isOriginalState = true;
        }

        //�ж϶�������
        if (StatePlay && digital.Equals("1"))
        {
            RandomPlay = Random.Range(1, 5);
            if (isOriginalState)
            {
                animator.SetInteger("State", RandomPlay);
                isOriginalState = false;
            }
            StatePlay = false;
        }
        else if (digital.Equals("0"))
        {
            animator.SetInteger("State", 0);
        }

        //�����ɲ��Ž���

    }

    //�رմ��ڡ��߳�
    private void OnApplicationQuit()
    {
        thread.Abort();
        if (isConnected)
            port.Close();
    }
}
