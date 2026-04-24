using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentViewManager : BaseViewManager
{
    public PersistentViewSetting PersistentViewSetting{get; private set;}

    protected override void Init(){
        base.Init();
        PersistentViewSetting = Hub.Resources.Get<PersistentViewSetting>("PersistentViewSetting");
        PersistentViewSetting.Init();
    }

    /// <summary>
    /// 打开常驻视图
    /// </summary>
    public void OpenPersistentView<T>(Action<T> onCreatePanel = null)
    where T : BaseUIControl, IBasePersistentViewControl
    {
        OpenPanel(onCreatePanel);
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
