using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayerOnEnable : MonoBehaviour
{
    public string assetsPath;
    private void OnEnable()
    {
        if (FrameworkManager.isInitFinish)
        {
            Hub.Audio.PlaysoundBg(assetsPath);
        }
    }
}
