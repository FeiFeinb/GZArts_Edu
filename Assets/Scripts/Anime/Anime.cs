using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TTR), typeof(AnimationControllerManager))]
public class Anime : MonoBehaviour
{
    //接受消息
    private string digital;
    private string analog;

    [Header("动画相关")]
    public Animator animator;
    private AnimatorStateInfo stateInfo;
    //随机的父动画
    private int FuAnimeState = 0;
    private int PlayTimes = 0;
    //父动画的子动画过程
    public int SonAnimeState = -1;
    public bool Initial, state, blossom, fuulblossom;
    public float InvertedSpeed;

    [Header("电压参数相关")]
    //在达到阈值后，允许的电压波动误差,暂时设为25mv
    public float voltageError = 25;
    //存nS内的电压波动，判断是否为客观因素导致
    private List<float> ErrorV = new List<float>();
    //主观性的电压变化
    public bool isInitiative;
    //需要个存各子动画的状态切换阈值

    //动画参数相关
    private bool isOriginalState;
    List<int> AnimeList = new List<int>() { 1, 2, 3, 4 };
    private bool resetList = true;
    //判断是否能切换动画（动画播放完毕
    private bool canChange = true;

    //状态机名字需改
    private void FixedUpdate()
    {
        Fuction();
        //AnimeControl();
        AnimeListControl();
        TestAnime();
    }

    //动态获取数值
    private void Fuction()
    {
        digital = TTR.Instance.digital;

        analog = TTR.Instance.analog;

        if (animator == null)
        {
            Debug.LogWarning("需要animator组件");
            return;
        }

        if (!TTR.Instance.isConnected)
            return;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    //测试动画
    private void TestAnime()
    {
        //框架可行，bool换成integer
        Debug.Log(digital);
        if (digital.Equals("1"))
        {
            animator.SetBool("Start", true);
            if (stateInfo.IsName("AnimationOne"))
            {
                animator.SetFloat("Inverted", 1);
                if (stateInfo.normalizedTime >= 1)
                {
                    animator.SetBool("Over", true);
                }
            }
        }
        else if (digital.Equals("0"))
        {
            animator.SetBool("Over", false);
            animator.SetFloat("Inverted", InvertedSpeed);
            if (stateInfo.normalizedTime <= 0 && stateInfo.IsName("AnimationOne"))
            {
                animator.SetBool("Start", false);
            }
        }
    }

    //洗牌
    private void AnimeListControl()
    {
        if (PlayTimes >= 4)
        {
            resetList = true;
            PlayTimes = 0;
        }

        if (resetList)
        {
            //10次循环，降低重复率
            for (int i = 0; i < 10; i++)
            {
                int m = Random.Range(0, 4);
                int n = Random.Range(0, 4);

                int temp;
                temp = AnimeList[m];
                AnimeList[m] = AnimeList[n];
                AnimeList[n] = temp;
            }

            resetList = false;
        }
    }

    //动画各参数的判断
    private void AnimeControl()
    {
        if (digital.Equals("0"))
        {
            animator.SetFloat("Inverted", 1);
            //需要过渡动画？
            animator.SetInteger("State", 0);
            return;
        }
        else
        {
            //换到别处
            animator.SetFloat("Inverted", InvertedSpeed);
        }

        //动画名字获取过慢，所以用以下方式解决 => 写成函数，方便重复利用
        if (stateInfo.IsName("State") && !isOriginalState)
        {
            isOriginalState = true;
        }

        //判断动画播放
        //判断过慢
        if (isOriginalState && digital.Equals("1"))
        {
            //判断该播放的动画
            //换成函数
            animator.SetInteger("State", AnimeList[FuAnimeState < 4 ? FuAnimeState : 0]);
        }

        //判断是否切换完成，待改（动画名称
        if (isOriginalState && !stateInfo.IsName("State"))
        {
            PlayTimes++;
            FuAnimeState++;
            isOriginalState = false;
        }

        //动画播放过程时的状态切换
        //需要得到各动画的切换数据
        //转换回待机状态动画时的切换时间与UI动画（烟雾散开？
        //下次动画开始最少间隔时间的时间（烟雾散开的时间？
    }

    //子动画切换，应该需要更多参数，例如个数
    /// <param name="change">动画切换的判断</param>
    /// <param name="toStateString">目的动画的切换参数</param>
    /// <param name="toState">切换参数</param>
    private void Son_AnimeChange(ref bool change, string toStateString, int toState)
    {
        canChange = false;

        animator.SetInteger(toStateString, toState);


    }

    //父动画切换
    private void Fu_AnimeChange()
    {

    }
}
