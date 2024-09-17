using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window_MVP<T1, T2, T3>: Window_VP<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BaseWindow_P
{
    protected static T1 window_M;
    protected override void CreateUiPrefab(string name){
        window_M = Activator.CreateInstance<T1>();
        base.CreateUiPrefab(name);
    }
    public override void Destroy(){
        base.Destroy();
        window_M = null;
    }
}


public abstract class Window_VP<T2, T3>:BaseWindow_P
where T2 : BaseUI_V
where T3 : BaseWindow_P
{
    protected T2 window_V;
    private static T3 instance;
    public static T3 Instance 
    {
        get 
        {
            instance ??= WindowController.GetWindow<T3>(typeof(T3).Name);
            return instance;
        }
    }

    public override void Destroy(){
        base.Destroy();
        ((IBaseUI_V)window_V).Destroy();
        Hub.Window.DestroyWindow(typeof(T3).Name);
    }

    public override void DownLaye(){
        window_V.transform.SetParent(hideWindow);
    }

    public override void SetToTopLayer(){
        window_V.transform.SetParent(showWindow);
        window_V.transform.SetSiblingIndex(0);
    }

    protected override void StartShow()
    {
        base.StartShow();
        window_V.transform.SetParent(showWindow);
    }

    protected override void HideFinish()
    {
        if(!isHideFinishDestroy){
            window_V.transform.SetParent(hideWindow);
        }
        base.HideFinish();
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    protected override void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name), showWindow);
        isCreate = true;
        window_V = prefab.GetComponent<T2>();
        ((IBaseUI_V)window_V).Init(Instance);
    }
}

public abstract class BaseWindow_P : BaseUI_P, IBaseWindow_P
{
    protected readonly Transform hideWindow = Hub.Window.inactiveWindow;
    protected readonly Transform showWindow = Hub.Window.activeWindow;
    private Action hideFinishCallBack;
    private string windowName;
    public string WindowName{get{return windowName; }}

    public abstract void DownLaye();
    public abstract void SetToTopLayer();

    /// <summary>
    /// 显示
    /// </summary>
    void IBaseUI_P.Show(){
        StartShow();
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    void IBaseWindow_P.Hide(Action _hideFinishCallBack){
        hideFinishCallBack = _hideFinishCallBack;
        StartHide();
    }

    /// <summary>
    /// 显示完成
    /// </summary>
    protected override void ShowFinish()
    {
        if(isShowFinish){
            LogManager.LogError($"Window界面：{WindowName}调用了多次打开完成方法，请检查");
        }
        base.ShowFinish();
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected override void HideFinish(){
        if(isCloseFinish){
            LogManager.LogError($"Window界面：{WindowName}调用了多次隐藏完成方法，请检查");
        }
        hideFinishCallBack?.Invoke();
        base.HideFinish();
    }

    protected override GameObject SetPrefabInfo(string _windowName){
        var prefabInfo = FrameworkManager.Window.GetPrefabInfo(_windowName);
        windowName = _windowName;
        isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        return prefabInfo.prefab;
    }
}
