using System;
using UnityEngine;

namespace GameControll
{
    public class DisplayController : BaseSingletonWithMono<DisplayController>
    {
        private void Awake()
        {
            for (int i = 0; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
                Screen.SetResolution(Display.displays[i].renderingHeight * 4 / 3, Display.displays[i].renderingHeight, true);
            }
        }
    }
}