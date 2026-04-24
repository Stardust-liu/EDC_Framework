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

    protected override async UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent_2DUI, RectTransform Parent_3DUI)
    {
        base.CreatePanel(uIPrefabInfo, Parent_2DUI, Parent_3DUI).Forget();;
        if (uIPrefabInfo.prefab_2D != null)
        {
            var prefab_2D_RuntimeKey = uIPrefabInfo.prefab_2D;
            await AssetManager.Init(out assetManager, prefab_2D_RuntimeKey).Load();
            var prefab = Hub.Resources.Get<GameObject>(prefab_2D_RuntimeKey);
            panel = GameObject.Instantiate(prefab, Parent_2DUI).GetComponent<T>();
            await ((IBaseUI)panel).Init(isHideFinishDestroy, is3DUI: false);
        }
        else
        {
            var prefab_3D_RuntimeKey = uIPrefabInfo.prefab_3D;
            await AssetManager.Init(out assetManager, prefab_3D_RuntimeKey).Load();
            var prefab = Hub.Resources.Get<GameObject>(prefab_3D_RuntimeKey);
            panel = GameObject.Instantiate(prefab, Parent_3DUI).GetComponent<T>();
            ((IBaseUI)panel).Init(isHideFinishDestroy, is3DUI: true);
        }
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

public class BaseUIControl<panel_2D, panel_3D> : BaseUIControl
where panel_2D : IBaseUI
where panel_3D : IBaseUI
{
    protected panel_2D panel2D;
    protected panel_3D panel3D;

    private int showFinishPanelCount;
    private AssetManager assetManager;

    protected override async UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent_2DUI, RectTransform Parent_3DUI)
    {
        base.CreatePanel(uIPrefabInfo, Parent_2DUI, Parent_3DUI).Forget();
        
        var prefab_2D_RuntimeKey = uIPrefabInfo.prefab_2D;
        var prefab_3D_RuntimeKey = uIPrefabInfo.prefab_3D;
        var keyNames = new List<string>(){prefab_2D_RuntimeKey, prefab_3D_RuntimeKey};
        await AssetManager.Init(out assetManager, keyNames).Load();
        var prefab_2D = Hub.Resources.Get<GameObject>(prefab_2D_RuntimeKey);
        var prefab_3D = Hub.Resources.Get<GameObject>(prefab_3D_RuntimeKey);
        panel2D = GameObject.Instantiate(prefab_2D, Parent_2DUI).GetComponent<panel_2D>();
        panel3D = GameObject.Instantiate(prefab_3D, Parent_3DUI).GetComponent<panel_3D>();
        await ((IBaseUI)panel2D).Init(isHideFinishDestroy, is3DUI: false);
        await ((IBaseUI)panel3D).Init(isHideFinishDestroy, is3DUI: true);
    }

    protected override void StartShow()
    {
        base.StartShow();
        showFinishPanelCount = 0;
        ((IBaseUI)panel2D).Open(ShowFinish);
        ((IBaseUI)panel3D).Open(ShowFinish);
    }

    protected override void StartHide()
    {
        base.StartHide();
        ((IBaseUI)panel2D).Close(HideFinish);
        ((IBaseUI)panel3D).Close(HideFinish);
    }

    protected override void ShowFinish()
    {
        showFinishPanelCount++;
        if(showFinishPanelCount == 2){
            isShowFinish = true;
        }
    }

    protected override void HideFinish()
    {
        showFinishPanelCount--;
        if (showFinishPanelCount == 0)
        {
            isHideFinish = true;
            hideFinishCallBack?.Invoke();
            hideFinishCallBack = null;
        }
    }

    protected override void DestroyPanel()
    {
        ((IBaseUI)panel2D).DestroyPanel();
        ((IBaseUI)panel3D).DestroyPanel();
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

    async UniTask IBaseUIControl.CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent_2DUI, RectTransform Parent_3DUI)
    {
        await CreatePanel(uIPrefabInfo, Parent_2DUI, Parent_3DUI);
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
    
    protected virtual UniTask CreatePanel(UIPrefabInfo uIPrefabInfo, RectTransform Parent_2DUI, RectTransform Parent_3DUI)
    {
        isHideFinishDestroy = uIPrefabInfo.isHideFinishDestroy;
        return UniTask.CompletedTask;
    }
}
