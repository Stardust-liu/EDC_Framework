using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseWindow_P : IBaseUI_P
{
    void Hide(Action hideFinishCallBack);
}
