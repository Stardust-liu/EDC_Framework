using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
public abstract class BaseUI<Model> : BaseUI
where Model : BaseUI_Model, new()
{
    protected Model model;
    protected override void Init()
    {
        base.Init();
        model = new Model();
        ((IBaseUI_Model)model).Init();
    }

    protected override void DestroyPanel()
    {
        model = null;
        base.DestroyPanel();
    }
}
[RequireComponent(typeof(Animator))]
public abstract class BaseUI : MonoBehaviour, IBaseUI, ISendCommand, ISendQuery, IBindEvent
{
    public Animator uiAnimator;
    public LocalizationFileGroup localizationFileGroup;
    private Action showFinishCallBack;
    private Action hideFinishCallBack;
    private bool isHideFinishDestroy;
    private bool is3DUI;

    void IBaseUI.Init(bool _isHideFinishDestroy, bool _is3DUI)
    {
        isHideFinishDestroy = _isHideFinishDestroy;
        is3DUI = _is3DUI;
        localizationFileGroup?.Loadnfo();
        Init();
    }

    void IBaseUI.Open(Action _showFinishCallBack)
    {
        showFinishCallBack = _showFinishCallBack;
        StartShow();
    }
    void IBaseUI.Close(Action _hideFinishCallBack)
    {
        hideFinishCallBack = _hideFinishCallBack;
        StartHide();
    }

    void IBaseUI.DestroyPanel()
    {
        DestroyPanel();
    }


    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init() { }

    /// <summary>
    /// 准备打开
    /// </summary>
    protected virtual void StartShow()
    {
        MoveToShowParent();
        PLayShowAnimator();
        UpdateMargins();
        this.AddListener<UpdateMargins>(UpdateMargins);
    }

    /// <summary>
    /// 准备隐藏
    /// </summary>
    protected virtual void StartHide()
    {
        PLayHideAnimator();
        this.RemoveListener<UpdateMargins>(UpdateMargins);
    }

    /// <summary>
    /// 打开完成
    /// </summary>
    protected virtual void ShowFinish()
    {
        showFinishCallBack?.Invoke();
        showFinishCallBack = null;
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected virtual void HideFinish()
    {
        hideFinishCallBack?.Invoke();
        hideFinishCallBack = null;
        if (isHideFinishDestroy)
        {
            ((IBaseUI)this).DestroyPanel();
        }
        else
        {
            MoveToHideParent();
        }
    }

    protected void MoveToParent(PanelManager panelManager, bool isShow)
    {
        if (isShow)
        {
            var parent = is3DUI ? panelManager.Parent_3DUI : panelManager.Parent_2DUI;
            if (transform.parent != parent)
            {
                transform.SetParent(parent, false);
            }
        }
        else
        {
            var parent = is3DUI ? panelManager.Parent_3DUI_Hide : panelManager.Parent_2DUI_Hide;
            transform.SetParent(parent, false);
        }
    }

    /// <summary>
    /// 销毁界面
    /// </summary>
    protected virtual void DestroyPanel()
    {
        Destroy(gameObject);
    }

    protected virtual void UpdateMargins() {}
    protected abstract void MoveToShowParent();
    protected abstract void MoveToHideParent();

    private void PLayShowAnimator()
    {
        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger("Show");
        }
    }

    private void PLayHideAnimator()
    {
        if (uiAnimator != null)
        {
            uiAnimator.SetTrigger("Hide");
        }
    }
}

