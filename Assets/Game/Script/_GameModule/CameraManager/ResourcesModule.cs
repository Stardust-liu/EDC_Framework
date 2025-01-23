using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesModule
{
    private readonly ABManager aBManager;
    public ResourcesModule(){
        aBManager = new ABManager();
        aBManager.LoadAbFile("scriptableobject");
        aBManager.LoadAbFile("soundBg");
        aBManager.LoadAbFile("soundEffect");
        aBManager.LoadAbFile("soundDialogue");
        aBManager.LoadAbFile("csv");
        aBManager.LoadAbFile("sprite");
        aBManager.LoadAbFile("prefab");
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    private void UnloadResources(){
        Resources.UnloadUnusedAssets();
    }
#region 脚本化对象
    /// <summary>
    /// 获取脚本化对象
    /// </summary>
    public T GetScriptableobject<T>(string fileNmae) where T : ScriptableObject{
        return aBManager.LoadAsset<T>("scriptableobject", fileNmae);
    }
#endregion
#region 音频文件
    /// <summary>
    /// 获取背景音
    /// </summary>
    public AudioClip GetSoundBg(string fileNmae){
        return aBManager.LoadAsset<AudioClip>("soundBg", fileNmae);
    }

    /// <summary>
    /// 获取音效
    /// </summary>
    public AudioClip GetSoundEffect(string fileNmae){
        return aBManager.LoadAsset<AudioClip>("soundEffect", fileNmae);
    }

    /// <summary>
    /// 获取对话音效
    /// </summary>
    public AudioClip GetSoundDialogue(string fileNmae){
        return aBManager.LoadAsset<AudioClip>("soundDialogue", fileNmae);
    }
#endregion
#region 文本文件
    /// <summary>
    /// 获取CSV文件
    /// </summary>
    public TextAsset GetCsvFile(string fileNmae){
        return aBManager.LoadAsset<TextAsset>("csv", fileNmae);
    }
#endregion
#region 图像文件
    /// <summary>
    /// 获取Sprite文件
    /// </summary>
    public Sprite GetSprite(string fileNmae){
        return aBManager.LoadAsset<Sprite>("sprite", fileNmae);
    }
#endregion
#region 预制体
    /// <summary>
    /// 获取预制体
    /// </summary>
    public GameObject GetPrefab(string fileNmae){
        return aBManager.LoadAsset<GameObject>("prefab", fileNmae);
    }
#endregion
}
