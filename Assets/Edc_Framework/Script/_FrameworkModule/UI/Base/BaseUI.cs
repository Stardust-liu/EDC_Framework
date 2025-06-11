using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public abstract class BaseUI<Model>: BaseUI
where Model : BaseUI_Model, new()
{
    protected Model view_M;
    protected override void Init(){
        base.Init();
        view_M = new Model();
        ((IBaseUI_Model)view_M).Init();
    }

    public override void OnDestroy(){
        base.OnDestroy();
        view_M = null;
    }
}
public abstract class BaseUI : MonoBehaviour, IBaseUI, IUIType, ISendCommand, ISendQuery, IBindEvent
{
    [FoldoutGroup("TweenGroup")]
    public TweenGroup tweenGroupIn;
    [FoldoutGroup("TweenGroup")]
    public TweenGroup tweenGroupOut;
    protected bool is3DUI;
    protected bool isHideFinishDestroy;
    private bool isShow;
    protected bool isShowFinish;
    protected bool isHideFinish;
    public bool Is3DUI{get{return is3DUI;}}
    public bool IsShow{get{return isShow;}}
    public bool IsShowFinish{get{return isShowFinish;}}
    public bool IsHideFinish{get{return isHideFinish;}}
    private Action hideFinishCallBack;
    // private static readonly int activeLayer = LayerMask.NameToLayer("UI");
    // private static readonly int inactiveLayer = LayerMask.NameToLayer("UI_Hide");

    void IBaseUI.Open(){
        StartShow();
    }
    void IBaseUI.Close(Action _hideFinishCallBack){
        hideFinishCallBack = _hideFinishCallBack;
        StartHide();
    }

    void IBaseUI.Init(UIPrefabInfo uIPrefabInfo)
    {
        is3DUI = uIPrefabInfo.is3DUI;
        isHideFinishDestroy = uIPrefabInfo.isHideFinishDestroy;
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init(){
        tweenGroupIn?.SetAllTweenToStart();
    }

    /// <summary>
    /// 准备打开
    /// </summary>
    protected virtual void StartShow()
    {
        isShow = true;
        isHideFinish = isShowFinish = false;
        //gameObject.layer = activeLayer;
        tweenGroupIn?.Play();
        tweenGroupIn?.AddListenerTween(ShowFinish);
        if(tweenGroupIn == null){
            ShowFinish();
        }
        this.AddListener<UpdateMargins>(UpdateMargins);
        UpdateMargins();
    }

    /// <summary>
    /// 准备隐藏
    /// </summary>
    protected virtual void StartHide(){
        isShow = false;
        isShowFinish = isHideFinish = false;
        tweenGroupOut?.Play();
        tweenGroupOut?.AddListenerTween(HideFinish);
        if(tweenGroupOut == null){
            HideFinish();
        }
        this.RemoveListener<UpdateMargins>(UpdateMargins);
    }

    /// <summary>
    /// 打开完成
    /// </summary>
    protected virtual void ShowFinish(){
        isShowFinish = true;
        tweenGroupIn?.ClearAllTweenCompleteListener();
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected virtual void HideFinish(){
        isHideFinish = true;
        tweenGroupOut?.ClearAllTweenCompleteListener();
        hideFinishCallBack.Invoke();
        hideFinishCallBack = null;
        if (isHideFinishDestroy){
            OnDestroy();
        }
        else{
            //gameObject.layer = inactiveLayer;
        }
    }

    /// <summary>
    /// 销毁界面
    /// </summary>
    public virtual void OnDestroy(){
        Destroy(gameObject);
    }

    protected virtual void UpdateMargins(){}
}
