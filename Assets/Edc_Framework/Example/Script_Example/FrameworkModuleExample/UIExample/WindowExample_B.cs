using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ResourceKey("WindowExample_B")]
public class WindowExample_B : BaseWindow
{
    public Button close_Btn;

    protected override void Init()
    {
        base.Init();
        close_Btn.onClick.AddListener(ClickClose);
    }
    
    private void ClickClose()
    {
        Hub.Window.CloseWindow();
    }
}
