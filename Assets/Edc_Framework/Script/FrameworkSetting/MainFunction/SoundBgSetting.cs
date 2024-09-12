using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BGM_Setting", menuName = "创建.Assets文件/BGM_Setting")]
public class SoundBgSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel ="音频名字", ValueLabel ="音频文件")]
    public Dictionary<string, AudioClip> universalSound;

    public AudioClip GetSoundBg(string audioName, string sceneName = null){
        if(sceneName == null){
            return universalSound[audioName];
        }
        switch (sceneName)
        {
            default:
                return null;
        }
    }
}
