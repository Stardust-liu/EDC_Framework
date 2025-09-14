using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseViewManager : PanelManager, IBindEvent
{
    public RectTransform rectTransformManager;
    public RectTransform rectTransformManager_3D;
    private const int margin = 100;

    protected override void Init()
    {
        base.Init();
        this.AddListener<UpdateMargins>(SetMargins);
    }

    /// <summary>
    /// 设置页边距
    /// </summary>
    private void SetMargins()
    {
        var UIManager = Hub.UI;
        var horizontalMargin = Mathf.Lerp(margin,  0, UIManager.HorizontalMargin);
        var verticalMargin = Mathf.Lerp(margin,  0, UIManager.VerticalMargin);

        var offsetMin = new Vector2(horizontalMargin, verticalMargin);
        var offsetMax = new Vector2(-horizontalMargin, -verticalMargin);

        rectTransformManager_3D.offsetMin = rectTransformManager.offsetMin = offsetMin;
        rectTransformManager_3D.offsetMax = rectTransformManager.offsetMax = offsetMax;
    }
}
