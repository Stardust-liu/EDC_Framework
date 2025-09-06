using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene_EditorStart : BaseEditorStartControl
{
#if UNITY_EDITOR
    public override void Init()
    {
        var view = Hub.View;
        if (!view.CheckCurrentView<MenuView_2D>())
        {
            //view.ChangeView<MenuView>();
        }
    }
#endif
}
