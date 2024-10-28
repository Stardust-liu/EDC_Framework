using System.Collections;
using System.Collections.Generic;
using Example;
using UnityEngine;
using UnityEngine.UI;

public class View3_V : BaseUI_V<View3_P>
{
    public Button openWindow1;

    protected override void Start()
    {
        openWindow1.onClick.AddListener(presenter.OpenWindow1);
    }
}
