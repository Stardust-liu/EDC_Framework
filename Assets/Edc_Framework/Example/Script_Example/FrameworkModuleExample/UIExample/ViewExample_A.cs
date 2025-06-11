using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ResourceKey("ViewExample_A")]
public class ViewExample_A : BaseView
{
    public Button changeView_BBtn;

    protected override void Init()
    {
        base.Init();
        changeView_BBtn.onClick.AddListener(ClickChangeView_B);
    }

    private void ClickChangeView_B()
    {
        Hub.View.ChangeView<ViewExample_B>();
    }
}
