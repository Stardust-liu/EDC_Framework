using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene_EnterControl : EnterSceneControl
{
    public override void Init()
    {
        var view = Hub.View;
        if (!view.CheckCurrentView<MenuView_C>())
        {
            view.ChangeView<MenuView_C>();
        }
    }
}
