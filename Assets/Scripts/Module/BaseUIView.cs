using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUIView : MonoBehaviour
{
    public bool isActive => gameObject.activeSelf;
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
