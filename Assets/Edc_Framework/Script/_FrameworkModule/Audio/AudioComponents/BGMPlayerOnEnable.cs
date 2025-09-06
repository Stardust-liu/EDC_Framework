using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayerOnEnable : MonoBehaviour
{
    public string fileNmae_AB;
    public string assetsPath;
    private void OnEnable()
    {
        if (FrameworkManager.isInitFinish)
        {
            Hub.Audio.PlaysoundBg(new ResourcePath(fileNmae_AB, assetsPath));
        }
    }
}
