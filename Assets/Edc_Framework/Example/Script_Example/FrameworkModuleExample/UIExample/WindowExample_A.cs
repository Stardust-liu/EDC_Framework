using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ResourceKey("WindowExample_A")]
public class WindowExample_A : BaseWindow
{
    public Button openWindow_BBtn;
    public Button close_Btn;

    protected override void Init()
    {
        base.Init();
        openWindow_BBtn.onClick.AddListener(ClickOpenWindow_B);
        close_Btn.onClick.AddListener(ClickClose);
    }

    private void ClickOpenWindow_B()
    {
        Hub.Window.OpenWindow<WindowExample_B>();
    }

    private void ClickClose()
    {
        Hub.Window.CloseWindow();
    }
}
