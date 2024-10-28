using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class View_MVP<Model, View, Presenter>: View_VP<View, Presenter>
where Model : class, new()
where View : BaseUI_V<Presenter>
where Presenter : BaseView_P
{
    protected static Model view_M;
    protected override void CreateUiPrefab(string name){
        view_M = Activator.CreateInstance<Model>();
        base.CreateUiPrefab(name);
    }

    public override void Destroy(){
        base.Destroy();
        view_M = null;
    }
}

public abstract class View_VP<View,Presenter>:BaseView_P
where View : BaseUI_V<Presenter>
where Presenter : BaseView_P
{
    protected static View view_V;
    private static Presenter instance;
    public static Presenter Instance 
    {
        get 
        {
            instance ??= ViewController.GetView<Presenter>(typeof(Presenter).Name);
            return instance;
        }
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    protected override void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name));
        isCreate = true;
        view_V =  prefab.GetComponent<View>();
        view_V.tweenGroupIn.SetAllTweenToStart();
        ((IBaseUI_V)view_V).Init(Instance);
    }

    public override void Destroy(){
        base.Destroy();
        ((IBaseUI_V)view_V).Destroy();
        view_V = null;
        Hub.View.DestroyView(typeof(Presenter).Name);
    }

    protected override void StartShow()
    {
        base.StartShow();
        if(is3DUI){
            view_V.transform.SetParent(showViewParent_3DUI, false);
        }
        else{
            view_V.transform.SetParent(showViewParent_UI, false);
        }
        view_V.tweenGroupIn.Play();
        view_V.tweenGroupIn.AddListenerTween(ShowFinish);
        view_V.StartShow();
    }

    protected override void StartHide()
    {
        base.StartHide();
        view_V.tweenGroupOut.Play();
        view_V.tweenGroupOut.AddListenerTween(HideFinish);
        view_V.StartHide();
    }

    protected override void ShowFinish()
    {
        base.ShowFinish();
        view_V.ShowFinish();
        view_V.tweenGroupIn.ClearAllTweenCompleteListener();
    }

    protected override void HideFinish()
    {
        view_V.HideFinish();
        view_V.tweenGroupOut.ClearAllTweenCompleteListener();
        if(!isHideFinishDestroy){
            if(is3DUI){
                view_V.transform.SetParent(hideViewParent_3DUI, false);
            }
            else{
                view_V.transform.SetParent(hideViewParent_UI, false);
            }
        }
        base.HideFinish();
    }
}

public abstract class BaseView_P : BaseUI_P, IBaseView_P
{
    protected readonly Transform hideViewParent_3DUI = Hub.View.inactiveView_3DUI;
    protected readonly Transform showViewParent_3DUI = Hub.View.activeView_3DUI;
    protected readonly Transform hideViewParent_UI = Hub.View.inactiveView_UI;
    protected readonly Transform showViewParent_UI = Hub.View.activeView_UI;
    private Action<BaseView_P> hideFinishCallBack;
    private BaseView_P nextShowView;
    private string viewName;
    public string ViewName{get{return viewName; }}
    public virtual void PreShowEffect(){}

    /// <summary>
    /// 显示
    /// </summary>
    void IBaseUI_P.Show(){
        StartShow();
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    void IBaseView_P.Hide(){
        StartHide();
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    void IBaseView_P.Hide(Action<BaseView_P> _hideFinishCallBack, BaseView_P _nextView){
        hideFinishCallBack = _hideFinishCallBack;
        nextShowView = _nextView;
        StartHide();
    }

    /// <summary>
    /// 显示完成
    /// </summary>
    protected override void ShowFinish()
    {
        if(isShowFinish){
            LogManager.LogError($"View界面：{ViewName}调用了多次打开完成方法，请检查");
        }
        base.ShowFinish();
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected override void HideFinish(){
        if(isCloseFinish){
            LogManager.LogError($"View界面：{ViewName}调用了多次隐藏完成方法，请检查");
        }
        hideFinishCallBack?.Invoke(nextShowView);
        nextShowView = null;
        hideFinishCallBack = null;
        base.HideFinish();
    }

    protected override GameObject SetPrefabInfo(string _viewName){
        var prefabInfo = ViewController.View.GetPrefabInfo(_viewName);
        viewName = _viewName;
        isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        is3DUI = prefabInfo.isHideFinishDestroy;
        return prefabInfo.prefab;
    }
}
