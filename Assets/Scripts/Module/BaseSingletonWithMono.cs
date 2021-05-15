using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingletonWithMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            //可能已经挂载到场景中，先查找
            if (_instance == null)
                _instance = FindObjectOfType<T>();
            //查找不到，则自行创建一个
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).ToString());
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }
}
