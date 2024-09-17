using System;
using UnityEngine;

public abstract class View_MVP<T1, T2, T3>: View_VP<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BaseView_P
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

public abstract class View_VP<T2,T3>:BaseView_P
where T2 : BaseUI_V
where T3 : BaseView_P
{
    protected static T2 view_V;
    private static T3 instance;
    public static T3 Instance 
    {
        get 
        {
            instance ??= ViewController.GetView<T3>(typeof(T3).Name);
            return instance;
        }
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    protected override void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name), showviewParent);
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

public abstract class BaseView_P : BaseUI_P, IBaseView_P
{
    protected readonly Transform hideViewParent = Hub.View.inactiveView;
    protected readonly Transform showviewParent = Hub.View.activeView;
    private Action<BaseView_P> hideFinishCallBack;
    private BaseView_P nextShowView;
    private string viewName;
    public string ViewName{get{return viewName; }}

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
        return prefabInfo.prefab;
    }
}
