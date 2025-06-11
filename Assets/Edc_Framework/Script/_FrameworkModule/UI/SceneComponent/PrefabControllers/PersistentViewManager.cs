using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentViewManager : PanelManager
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
    public void OpenPersistentView<T>() where T : BasePersistentView
    {
        OpenPanel<T>();
    }

    /// <summary>
    /// 关闭常驻视图
    /// </summary>
    public void ClosePersistentView<T>(Action hideFinishCallBack = null) where T : BasePersistentView
    {
        ClosePanel<T>(hideFinishCallBack);
    }
   
    protected override UIPrefabInfo GetPanelInfo(string panelName)
    {
        return PersistentViewSetting.GetPanelInfo(panelName);
    }
}
