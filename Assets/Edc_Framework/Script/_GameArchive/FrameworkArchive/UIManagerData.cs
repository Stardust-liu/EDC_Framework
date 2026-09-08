using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using UnityEngine;

[ArchiveKey("UIManagerData")]
public class UIManagerData : BaseGameArchive
{
    public float horizontalMargin;
    public float verticalMargin;

    protected internal override void OnCreateDefaultData()
    {
        horizontalMargin = 1;
        verticalMargin = 1;
    }

    /// <summary>
    /// 设置水平页边距
    /// </summary>
    public void SetHorizontalMargin(float value){
        horizontalMargin = value;
        SetDirty();
    }


    /// <summary>
    /// 设置垂直页边距
    /// </summary>
    public void SetVerticalMargin(float value){
        verticalMargin = value;
        SetDirty();
    }
}
