using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBaseUI
{
    UniTask Init(bool isHideFinishDestroy, bool is3DUI);
    void Open(Action showFinishCallBack);
    void Close(Action hideFinishCallBack);
    void DestroyPanel();
}
