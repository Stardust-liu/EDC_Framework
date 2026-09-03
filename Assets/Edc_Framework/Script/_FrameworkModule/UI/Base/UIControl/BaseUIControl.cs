using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUIControl<T> : BaseUIControl
where T : IBaseUI
{
    public T panel;
    private IResourceOwner resourceOwner;

    protected override async UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform parent)
    {
        await base.CreatePanel(uIPrefabInfo, parent);
        var prefab_2D_RuntimeKey = uIPrefabInfo.prefab;
        resourceOwner = Hub.Resources.CreateOwner(GetType().Name);
        await resourceOwner.LoadAsset(prefab_2D_RuntimeKey);
        var prefab = resourceOwner.GetAsset<GameObject>(prefab_2D_RuntimeKey);
        panel = GameObject.Instantiate(prefab, parent).GetComponent<T>();
        await ((IBaseUI)panel).Init(IsShowFinishValid, IsHideFinishValid);
    }

    protected override void StartShow()
    {
        base.StartShow();
        ((IBaseUI)panel).Open(ShowFinish);
    }

    protected override void StartHide()
    {
        base.StartHide();
        ((IBaseUI)panel).Close(HideFinish);
    }

    protected override void ShowFinish()
    {
        isShowFinish = true;
    }

    protected override void HideFinish()
    {
        if (isShow)
        {
            return;
        }
        if (!isHideFinishDestroy)
        {
            ((IBaseUI)panel).CompleteHide();
        }
        isHideFinish = true;
        hideFinishCallBack?.Invoke();
        hideFinishCallBack = null;
    }

    protected override void DestroyPanel()
    {
        ((IBaseUI)panel).DestroyPanel();
        panel = default;
        resourceOwner?.ReleaseAll();
        resourceOwner = null;
    }

    private bool IsShowFinishValid()
    {
        return isShow;
    }

    private bool IsHideFinishValid()
    {
        return !isShow;
    }
}

public abstract class BaseUIControl : IBaseUIControl
{
    protected bool isShow;
    protected bool isShowFinish;
    protected bool isHideFinish;
    protected bool isHideFinishDestroy;
    public bool IsShow { get { return isShow; } }
    public bool IsShowFinish { get { return isShowFinish; } }
    public bool IsHideFinish { get { return isHideFinish; } }
    public bool IsHideFinishDestroy { get { return isHideFinishDestroy; } }
    protected Action hideFinishCallBack;

    async UniTask IBaseUIControl.CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent)
    {
        await CreatePanel(uIPrefabInfo, Parent);
    }

    void IBaseUIControl.Open()
    {
        StartShow();
    }

    void IBaseUIControl.Close(Action _hideFinishCallBack)
    {
        hideFinishCallBack = _hideFinishCallBack;
        StartHide();
    }

    void IBaseUIControl.DestroyPanel()
    {
        DestroyPanel();
    }

    protected virtual void StartShow()
    {
        isShow = true;
        isHideFinish = isShowFinish = false;
    }
    protected virtual void StartHide()
    {
        isShow = false;
        isShowFinish = isHideFinish = false;
    }

    protected abstract void ShowFinish();

    protected abstract void HideFinish();

    protected abstract void DestroyPanel();
    
    protected virtual UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent)
    {
        isHideFinishDestroy = uIPrefabInfo.isHideFinishDestroy;
        return UniTask.CompletedTask;
    }
}
