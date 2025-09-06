using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseOptionItem : BaseSelectItem
{
    public string[] localizationID;
    private Action<Transform, string[]> changeOptionItem;
    private Action exitBtn;

    protected void SetInitAction(Action<Transform, string[]> _changeOptionItem, Action _exitBtn)
    {
        changeOptionItem = _changeOptionItem;
        exitBtn = _exitBtn;
    }

    protected override void ChangeOptionItem()
    {
        changeOptionItem?.Invoke(selectEffectParent, localizationID);
    }

    protected override void ExitBtn()
    {
        exitBtn?.Invoke();
    }
}
