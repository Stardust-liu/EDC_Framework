using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseView_P : IBaseUI_P
{
    void Hide();
    void Hide(Action<BaseView_P> hideFinishCallBack, BaseView_P nextView);
}
