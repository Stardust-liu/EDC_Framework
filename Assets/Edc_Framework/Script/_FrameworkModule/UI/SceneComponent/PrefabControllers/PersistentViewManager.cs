using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentViewManager : BaseViewManager
{
    public PersistentViewSetting PersistentViewSetting{get; private set;}

    protected override void Init(){
        base.Init();
        var resourcePath = new ResourcePath("PersistentViewSetting","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/PersistentViewSetting.asset");
        PersistentViewSetting = Hub.Resources.GetScriptableobject<PersistentViewSetting>(resourcePath);
    }

    /// <summary>
    /// 打开常驻视图
    /// </summary>
    public void OpenPersistentView<T>(Action<T> onOpenInit = null)
    where T : BaseUIControl, IBasePersistentViewControl
    {
        OpenPanel(onOpenInit);
    }

    /// <summary>
    /// 关闭常驻视图
    /// </summary>
    public void ClosePersistentView<T>()
    where T : BaseUIControl, IBasePersistentViewControl
    {
        ClosePanel<T>(null);
    }
   
    protected override UIPrefabInfo GetPanelInfo(string panelName)
    {
        return PersistentViewSetting.GetPanelInfo(panelName);
    }
}
