using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using UnityEngine;

public class UIManagerData : BaseGameArchive
{
    public float horizontalMargin = 1;
    public float verticalMargin = 1;

    /// <summary>
    /// 设置水平页边距
    /// </summary>
    public void SetHorizontalMargin(float value){
        horizontalMargin = value;
    }


    /// <summary>
    /// 设置垂直页边距
    /// </summary>
    public void SetVerticalMargin(float value){
        verticalMargin = value;
    }
}
