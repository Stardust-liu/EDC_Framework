using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlaySoundEffect : MonoBehaviour
{
    public string assetsPath;

    public void PlaySound()
    {
        Hub.Audio.PlaySoundEffect(assetsPath);
    }
}
