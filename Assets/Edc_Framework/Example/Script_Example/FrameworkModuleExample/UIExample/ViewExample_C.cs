using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ResourceKey("ViewExample_C")]
public class ViewExample_C : BaseView
{
    public Button openWindow_ABtn;

    protected override void Init()
    {
        base.Init();
        openWindow_ABtn.onClick.AddListener(ClickOpenWindow_A);
    }
    
    private void ClickOpenWindow_A()
    {
        Hub.Window.OpenWindow<WindowExample_A>();
    }
}
