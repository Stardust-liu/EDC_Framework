using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseUIControl
{
    public bool IsShow{get;}
    public bool IsShowFinish {get;}
    public bool IsHideFinish{get;}
    public bool IsHideFinishDestroy{get;}
    void CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent_2DUI, RectTransform Parent_3DUI);
    void Open();
    void Close(Action hideFinishCallBack);
    void DestroyPanel();
}
