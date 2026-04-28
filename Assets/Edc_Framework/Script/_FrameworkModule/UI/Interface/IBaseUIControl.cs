using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBaseUIControl
{
    public bool IsShow{get;}
    public bool IsShowFinish {get;}
    public bool IsHideFinish{get;}
    public bool IsHideFinishDestroy{get;}
    UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent);
    void Open();
    void Close(Action hideFinishCallBack);
    void DestroyPanel();
}
