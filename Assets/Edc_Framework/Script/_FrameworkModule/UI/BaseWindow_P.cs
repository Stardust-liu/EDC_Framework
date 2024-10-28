using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window_MVP<Model, View, Presenter>: Window_VP<View, Presenter>
where Model : class, new()
where View : BaseUI_V<Presenter>
where Presenter : BaseWindow_P
{
    protected static Model window_M;
    protected override void CreateUiPrefab(string name){
        window_M = Activator.CreateInstance<Model>();
        base.CreateUiPrefab(name);
    }
    public override void Destroy(){
        base.Destroy();
        window_M = null;
    }
}


public abstract class Window_VP<View, Presenter>:BaseWindow_P
where View : BaseUI_V<Presenter>
where Presenter : BaseWindow_P
{
    protected View window_V;
    private static Presenter instance;
    public static Presenter Instance 
    {
        get 
        {
            instance ??= WindowController.GetWindow<Presenter>(typeof(Presenter).Name);
            return instance;
        }
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    protected override void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name));
        isCreate = true;
        window_V = prefab.GetComponent<View>();
        window_V.tweenGroupIn.SetAllTweenToStart();
        ((IBaseUI_V)window_V).Init(Instance);
    }

    public override void Destroy(){
        base.Destroy();
        ((IBaseUI_V)window_V).Destroy();
        window_V = null;
        Hub.Window.DestroyWindow(typeof(Presenter).Name);
    }

    public override void DownLaye(){
        if(is3DUI){
            window_V.transform.SetParent(hideWindow_3DUI, false);
        }
        else{
            window_V.transform.SetParent(hideWindow_UI, false);
        }
    }

    public override void SetToTopLayer(){
        if(is3DUI){
            window_V.transform.SetParent(showWindow_3DUI, false);
        }
        else{
            window_V.transform.SetParent(showWindow_UI, false);
        }
        window_V.transform.SetSiblingIndex(0);
    }

    protected override void StartShow()
    {
        base.StartShow();
        if(is3DUI){
            window_V.transform.SetParent(showWindow_3DUI, false);
        }
        else{
            window_V.transform.SetParent(showWindow_UI, false);
        }
        window_V.tweenGroupIn.Play();
        window_V.tweenGroupIn.AddListenerTween(ShowFinish);
        window_V.StartShow();
    }

    protected override void StartHide()
    {
        base.StartHide();
        window_V.tweenGroupOut.Play();
        window_V.tweenGroupOut.AddListenerTween(HideFinish);
        window_V.StartHide();
    }

    protected override void ShowFinish()
    {
        base.ShowFinish();
        window_V.ShowFinish();
        window_V.tweenGroupIn.ClearAllTweenCompleteListener();
    }

    protected override void HideFinish()
    {
        window_V.HideFinish();
        window_V.tweenGroupOut.ClearAllTweenCompleteListener();
        if(!isHideFinishDestroy){
            if(is3DUI){
                window_V.transform.SetParent(hideWindow_3DUI, false);
            }
            else{
                window_V.transform.SetParent(hideWindow_UI, false);
            }
        }
        base.HideFinish();
    }
}

public abstract class BaseWindow_P : BaseUI_P, IBaseWindow_P
{
    protected readonly Transform hideWindow_3DUI = Hub.Window.inactiveWindow_3DUI;
    protected readonly Transform showWindow_3DUI = Hub.Window.activeWindow_3DUI;
    protected readonly Transform hideWindow_UI = Hub.Window.inactiveWindow_UI;
    protected readonly Transform showWindow_UI = Hub.Window.activeWindow_UI;
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
        var prefabInfo = WindowController.Window.GetPrefabInfo(_windowName);
        windowName = _windowName;
        isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        is3DUI = prefabInfo.isHideFinishDestroy;
        return prefabInfo.prefab;
    }

    public void Close(){
        Hub.Window.CloseWindow();
    }
}
