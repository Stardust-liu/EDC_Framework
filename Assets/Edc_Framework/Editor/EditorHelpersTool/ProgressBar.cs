using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProgressBar
{
    private static int needProcesCount;
    private static int procesCompleteCount;

    /// <summary>
    /// 重置进度
    /// </summary>
    public void ResetProgress(){
        procesCompleteCount = 0;
    }

    /// <summary>
    /// 增加进度
    /// </summary>
    public void AddProgress(int count = 1){
        procesCompleteCount += count;
    }

    /// <summary>
    /// 设置总进度
    /// </summary>
    public void SetNeedProcesCount(int _needProcesCount){
        needProcesCount = _needProcesCount;
    }

    /// <summary>
    /// 更新进度
    /// </summary>
    public void UpdateProgress(string title, string desc){
        EditorUtility.DisplayProgressBar(title, desc, (float)procesCompleteCount/needProcesCount);
    }

    /// <summary>
    /// 关闭进度条
    /// </summary>
    public void CloseProgressBar(){
        EditorUtility.ClearProgressBar();
    }
}
