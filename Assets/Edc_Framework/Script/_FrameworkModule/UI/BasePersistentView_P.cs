using System;
using UnityEngine;


public abstract class PersistentView_MVP<T1, T2, T3>: PersistentView_VP<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BasePersistentView_P
{
    protected static T1 view_M;
    protected override void CreateUiPrefab(string name){
        view_M = Activator.CreateInstance<T1>();
        base.CreateUiPrefab(name);
    }

    public override void Destroy(){
        base.Destroy();
        view_M = null;
    }
}

public abstract class PersistentView_VP<T2,T3>:BasePersistentView_P
where T2 : BaseUI_V
where T3 : BasePersistentView_P
{
    protected static T2 view_V;
    private static T3 instance;
    public static T3 Instance 
    {
        get 
        {
            instance ??= PersistentViewController.GetView<T3>(typeof(T3).Name);
            return instance;
        }
    }
    private readonly Transform hideViewParent = Hub.View.inactiveView;
    private readonly Transform showviewParent = Hub.View.activeView;
    private readonly Transform persistentViewParent = Hub.PersistentView.activePersistentView;


    /// <summary>
    /// 创建UI
    /// </summary>
    protected override void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name), persistentViewParent);
        isCreate = true;
        view_V =  prefab.GetComponent<T2>();
        ((IBaseUI_V)view_V).Init(Instance);
    }

    protected override void StartShow()
    {
        base.StartShow();
        view_V.transform.SetParent(showviewParent);
    }

    protected override void HideFinish()
    {
        if(!isHideFinishDestroy){
            view_V.transform.SetParent(hideViewParent);
        }
        base.HideFinish();
    }

    public override void Destroy(){
        base.Destroy();
        ((IBaseUI_V)view_V).Destroy();
        Hub.View.DestroyView(typeof(T3).Name);
    }
}

public abstract class BasePersistentView_P : BaseUI_P, IBasePersistentView_P
{
    protected readonly Transform hidePersistentViewParent = Hub.PersistentView.inactivePersistentView;
    protected readonly Transform showPersistentViewParent = Hub.PersistentView.activePersistentView;
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
        return prefabInfo.prefab;
    }
}


