using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using UnityEngine;

public class UIManagerData : BaseGameArchive
{
    public int leftMargin;
    public int rightMargin;
    public int bottomMargin;
    public int topMargin;

    /// <summary>
    /// 设置左边距
    /// </summary>
    public void SetLeftMargin(int value){
        leftMargin = value;
    }

    /// <summary>
    /// 设置右边距
    /// </summary>
    public void SetRightMargin(int value){
        rightMargin = value;
    }


    /// <summary>
    /// 设置下边距
    /// </summary>
    public void SetBottomMargin(int value){
        bottomMargin = value;
    }

    /// <summary>
    /// 设置上边距
    /// </summary>
    public void SetTopMargin(int value){
        topMargin = value;
    }
}
