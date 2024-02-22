using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class View_MVC<T1, T2, T3>: View_VC<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BaseView_C
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

public abstract class View_VC<T2,T3>:BaseView_C
where T2 : BaseUI_V
where T3 : BaseView_C
{
    protected static T2 view_V;
    public static T3 Instance {get {return ViewManager.GetView<T3>(typeof(T3).Name);}}

    /// <summary>
    /// 创建UI
    /// </summary>
    public override void CreateUiPrefab(){
        GameObject prefab;
        if(!isPersistentView){
            prefab = GameObject.Instantiate(this.prefab, ViewManager.parent);
        }
        else{
            prefab = GameObject.Instantiate(this.prefab, ViewManager.persistentViewParent);
        }
        this.prefab = null;
        isCreate = true;
        view_V =  prefab.GetComponent<T2>();
    }

    public override void Destroy(){
        base.Destroy();
        view_V.Destroy();
        Hub.View.DestroyView(typeof(T3).Name);
    }
}

public abstract class BaseView_C : BaseUI_C
{
    private string viewName;
    private float showNextViewWaitTime;
    private TransitionsType transitionsType;
    private Color fadeInColor;
    private Color fadeOutColor;
    protected bool isPersistentView;
    protected GameObject prefab;
    public string ViewName{get{return viewName; }}
    public float ShowNextViewWaitTime{get{return showNextViewWaitTime; }}
    public TransitionsType TransitionsType{get{return transitionsType; }}
    public Color FadeInColor{get{return fadeInColor; }}
    public Color FadeOutColor{get{return fadeOutColor; }}
    private static WaitForSeconds gradientWaitTime = new(WaitTime.viewGradientTime);

    public abstract void SetPrefabInfo();

    protected void SetPrefabInfo(string viewName, bool isPersistentView = false){
        this.viewName = viewName;
        this.isPersistentView = isPersistentView;
        if(!isPersistentView){
            var prefabInfo = ViewManager.View.GetPrefabInfo(viewName);
            prefab = prefabInfo.prefab;
            isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
            showNextViewWaitTime = prefabInfo.showNextViewWaitTime;
            transitionsType = prefabInfo.transitionsType;
            fadeInColor = prefabInfo.fadeInColor;
            fadeOutColor = prefabInfo.fadeOutColor;
        }
        else{
            var prefabInfo = ViewManager.View.GetPersistentPrefabInfo(viewName);
            prefab = prefabInfo.prefab;
            isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        }
    }

    protected override void PrepareForShwo(){
        base.PrepareForShwo();
        ApplyShowIfRequired();
    }

    protected override void PrepareForHide(){
        base.PrepareForShwo();
        ApplyFadeOutIfRequired();
    }


    /// <summary>
    /// 检测是否需要淡入效果
    /// </summary>
    private void ApplyShowIfRequired(){
        if(transitionsType == TransitionsType.FadeIn||
            transitionsType == TransitionsType.FadeInFadeOut){
                FrameworkManager.instance.StartCoroutine(WaitShwoFinish());
        }          
    }

    /// <summary>
    /// 检测是否需要淡出效果
    /// </summary>
    private void ApplyFadeOutIfRequired(){
        if(transitionsType == TransitionsType.FadeOut||
            transitionsType == TransitionsType.FadeInFadeOut){
                FrameworkManager.instance.StartCoroutine(WaitHideFinish());
        }
    }

    private IEnumerator WaitShwoFinish(){
        yield return gradientWaitTime;
        ShwoFinish();
    }

    /// <summary>
    /// 开始等待关闭完成
    /// </summary>
    public void StartWaitHideFinish(){
        FrameworkManager.instance.StartCoroutine(WaitHideFinish());
    }

    private IEnumerator WaitHideFinish(){
        yield return gradientWaitTime;
        HideFinish();
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
