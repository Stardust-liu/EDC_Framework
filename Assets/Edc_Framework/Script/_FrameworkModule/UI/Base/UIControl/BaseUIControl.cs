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
    private AssetManager assetManager;

    protected override async UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform parent)
    {
        base.CreatePanel(uIPrefabInfo, parent).Forget();;
        var prefab_2D_RuntimeKey = uIPrefabInfo.prefab;
        await AssetManager.Init(out assetManager, prefab_2D_RuntimeKey).Load();
        var prefab = Hub.Resources.Get<GameObject>(prefab_2D_RuntimeKey);
        panel = GameObject.Instantiate(prefab, parent).GetComponent<T>();
        await ((IBaseUI)panel).Init(isHideFinishDestroy);
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
        isHideFinish = true;
        hideFinishCallBack?.Invoke();
        hideFinishCallBack = null;
    }

    protected override void DestroyPanel()
    {
        ((IBaseUI)panel).DestroyPanel();
        assetManager.ReleaseAll();
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
