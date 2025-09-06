using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseUI
{
    void Init(bool isHideFinishDestroy, bool is3DUI);
    void Open(Action showFinishCallBack);
    void Close(Action hideFinishCallBack);
    void DestroyPanel();
}
