using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ResourceKeyAttribute("ConfirmWindow")]
public class ConfirmWindow_C : BaseWindowControl<ConfirmWindow_2D>
{
    public void InitInfo(string key, Action action)
    {
        panel.InitInfo(key, action);
    }
}
