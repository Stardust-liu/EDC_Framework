using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class View_MVP<T1, T2, T3>: View_VP<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BaseView_P
{
    protected static T1 view_M;
    public override void CreateUiPrefab(){
        base.CreateUiPrefab();
        view_M = Activator.CreateInstance<T1>();
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
            instance ??= ViewManager.GetView<T3>(typeof(T3).Name);
            return instance;
        }
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    public override void CreateUiPrefab(){
        GameObject prefab;
        if(!isPersistentView){
            prefab = GameObject.Instantiate(this.prefab, ViewManager.Parent);
        }
        else{
            prefab = GameObject.Instantiate(this.prefab, ViewManager.PersistentViewParent);
        }
        this.prefab = null;
        isCreate = true;
        view_V =  prefab.GetComponent<T2>();
        ((IBaseUI_V)view_V).Init(Instance);
    }

    public override void Destroy(){
        base.Destroy();
        ((IBaseUI_V)view_V).Destroy();
        Hub.View.DestroyView(typeof(T3).Name);
    }
}

public abstract class BaseView_P : BaseUI_P
{
    private string viewName;
    protected bool isPersistentView;
    protected GameObject prefab;
    public string ViewName{get{return viewName; }}
    public abstract void SetPrefabInfo();

    protected void SetPrefabInfo(string viewName, bool isPersistentView = false){
        this.viewName = viewName;
        this.isPersistentView = isPersistentView;
        if(!isPersistentView){
            var prefabInfo = ViewManager.View.GetPrefabInfo(viewName);
            prefab = prefabInfo.prefab;
            isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        }
        else{
            var prefabInfo = ViewManager.View.GetPersistentPrefabInfo(viewName);
            prefab = prefabInfo.prefab;
            isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        }
    }


    protected override void ShwoFinish()
    {
        if(isShowFinish){
            LogManager.LogError($"View界面：{ViewName}调用了多次打开完成方法，请检查");
        }
        base.ShwoFinish();
    }


    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected override void HideFinish(){
        if(isCloseFinish){
            LogManager.LogError($"View界面：{ViewName}调用了多次隐藏完成方法，请检查");
        }
        base.HideFinish();
    }
}
