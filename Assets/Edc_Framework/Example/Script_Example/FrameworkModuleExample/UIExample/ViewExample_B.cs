using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ResourceKey("ViewExample_B")]
public class ViewExample_B : BaseView
{
    public Button changeView_CBtn;

    protected override void Init()
    {
        base.Init();
        changeView_CBtn.onClick.AddListener(ClickChangeView_C);
    }
    
    private void ClickChangeView_C()
    {
        Hub.View.ChangeView<ViewExample_C>();
    }
}
