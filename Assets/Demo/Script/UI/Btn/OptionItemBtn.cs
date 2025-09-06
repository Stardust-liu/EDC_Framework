using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionItemBtn : BaseOptionItem
{
    public Button btn;
    public void SetInitAction(UnityAction btnClick, Action<Transform, string[]> _changeOptionItem, Action _exitBtn)
    {
        btn.onClick.AddListener(btnClick);
        SetInitAction(_changeOptionItem, _exitBtn);
    }
}
