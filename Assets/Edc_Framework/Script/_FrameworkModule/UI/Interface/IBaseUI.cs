using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseUI
{
    void Init(UIPrefabInfo uIPrefabInfo);
    void Open();
    void Close(Action hideFinishCallBack);
}

public interface IUIType {
    bool Is3DUI { get; }
}
