using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlaySoundEffect : MonoBehaviour
{
    public string fileNmae_AB;
    public string assetsPath;

    public void PlaySound()
    {
        if(!string.IsNullOrEmpty(fileNmae_AB) || !string.IsNullOrEmpty(assetsPath)){
            Hub.Audio.PlaySoundEffect(new ResourcePath(fileNmae_AB, assetsPath));
        }
    }
}
