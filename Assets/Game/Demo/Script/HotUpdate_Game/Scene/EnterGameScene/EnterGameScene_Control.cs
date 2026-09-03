using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class EnterGameScene_Control : EnterSceneControl
{
    public override void Init()
    {
        var view = Hub.View;
        var isShowLogo = FrameworkManager.IsShowLogo;
        if (isShowLogo)
        {
            
        }
        else
        {
            if (!view.CheckCurrentView<MenuView_C>())
            {
                view.ChangeView<MenuView_C>();
            }
        }
    }
}
