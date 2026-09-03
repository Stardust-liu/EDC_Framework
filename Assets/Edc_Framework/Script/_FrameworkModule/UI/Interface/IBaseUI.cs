using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBaseUI
{
    UniTask Init(Func<bool> isShowFinishValid, Func<bool> isHideFinishValid);
    void Open(Action showFinishCallBack);
    void Close(Action hideFinishCallBack);
    void DestroyPanel();
    void CompleteHide();
}
