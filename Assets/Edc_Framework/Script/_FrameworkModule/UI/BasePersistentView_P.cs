using System;
using Unity.VisualScripting;
using UnityEngine;


public abstract class PersistentView_MVP<Model, View, Presenter>: PersistentView_VP<View, Presenter>
where Model : class, new()
where View : BaseUI_V<Presenter>
where Presenter : BasePersistentView_P
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

public abstract class PersistentView_VP<View,Presenter>:BasePersistentView_P
where View : BaseUI_V<Presenter>
where Presenter : BasePersistentView_P
{
    protected static View view_V;
    private static Presenter instance;
    public static Presenter Instance 
    {
        get 
        {
            instance ??= PersistentViewController.GetView<Presenter>(typeof(Presenter).Name);
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

public abstract class BasePersistentView_P : BaseUI_P, IBasePersistentView_P
{
    protected readonly Transform hideViewParent_3DUI = Hub.PersistentView.inactivePersistentView_3DUI;
    protected readonly Transform showViewParent_3DUI = Hub.PersistentView.activePersistentView_3DUI;
    protected readonly Transform hideViewParent_UI = Hub.PersistentView.inactivePersistentView_UI;
    protected readonly Transform showViewParent_UI = Hub.PersistentView.activePersistentView_UI;
    private string persistentViewName;
    public string PersistentViewName{get{return persistentViewName; }}

    /// <summary>
    /// 显示
    /// </summary>
    void IBaseUI_P.Show(){
        StartShow();
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    void IBasePersistentView_P.Hide(){
        StartHide();
    }

    /// <summary>
    /// 显示完成
    /// </summary>
    protected override void ShowFinish()
    {
        if(isShowFinish){
            LogManager.LogError($"PersistentView界面：{PersistentViewName}调用了多次打开完成方法，请检查");
        }
        base.ShowFinish();
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected override void HideFinish(){
        if(isCloseFinish){
            LogManager.LogError($"PersistentView界面：{PersistentViewName}调用了多次隐藏完成方法，请检查");
        }
        base.HideFinish();
    }

    protected override GameObject SetPrefabInfo(string _viewName){
        var prefabInfo = PersistentViewController.PersistentView.GetPersistentPrefabInfo(_viewName);
        persistentViewName = _viewName;
        isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        is3DUI = prefabInfo.isHideFinishDestroy;
        return prefabInfo.prefab;
    }
}


