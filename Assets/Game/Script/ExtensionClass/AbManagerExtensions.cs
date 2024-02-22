using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbManagerExtensions
{
    /// <summary>
    /// 获取Scriptableobject对象
    /// </summary>
    public static T LoadScriptableobject<T>(this ABManager abManager, string fileNmae) where T : class{
        return abManager.LoadAsset<T>("scriptableobject", fileNmae);
    }
}
