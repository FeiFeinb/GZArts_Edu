using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(TTR))]
public class Anime : MonoBehaviour
{
    [Header("动画相关")]
    //随机的父动画
    public int FurAnimeState = 0;
    private int PlayTimes = 0;
    //父动画的子动画过程
    public int SonAnimeState = -1;
    public bool Initial, state, blossom, fuulblossom;
    public float InvertedSpeed;

    private bool isOriginalState;
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    List<int> AnimeList = new List<int>() {1, 2, 3, 4};
    private bool resetList = true;

    //电压一定时间内允许的波动误差(如果是模型动画
    //public float voltageError;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        AnimControl();
        AnimeListControl();
    }

    //洗牌
    private void AnimeListControl()
    {
        if(PlayTimes >= 4)
        {
            resetList = true;
            PlayTimes = 0;
        }

        if(resetList)
        {
            //在没有数组值的情况下，强行洗牌，内存消耗极大
            //for(int i = 0; i < int num; i++)
            //{
            //    int seed = Random.Range(1, 5);
            //    while(!AnimeList.Contains(seed))
            //    {
            //        AnimeList.Add(seed);
            //    }
            //}
            
            //10次循环，降低重复率
            for(int i = 0; i < 10; i++)
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

    private void AnimControl()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("State"))
        {
            isOriginalState = true;
            Debug.Log("1");
        }

        if (!TTR.Instance.isConnected)
            return;

        if (TTR.Instance.analog == "0")
        {
            animator.SetFloat("Inverted", 1);
            animator.SetInteger("State", 0);
            return;
        }
        else
        {
            animator.SetFloat("Inverted", InvertedSpeed);
        }

        //判断动画播放
        //判断过慢
        if (isOriginalState && TTR.Instance.digital.Equals("1"))
        {
            //判断该播放的动画
            animator.SetInteger("State", AnimeList[FurAnimeState = FurAnimeState < 4 ? FurAnimeState : 0]);
            PlayTimes++;
            FurAnimeState++;
            isOriginalState = false;
        }

        //动画播放进程
    }
}
