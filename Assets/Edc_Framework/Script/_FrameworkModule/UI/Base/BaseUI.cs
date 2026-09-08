using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour, IBaseUI, ISendCommand, ISendQuery, IBindEvent
{
    [SerializeField]
    private BaseUITransition uiTransition;
    public LocalizationFileGroup localizationFileGroup;
    private Action showFinishCallBack_Anim;
    private Action hideFinishCallBack_Anim;
    private Func<bool> isShowFinishValid;
    private Func<bool> isHideFinishValid;
    private bool isShowFinished;
    private bool isHideFinished;

    async UniTask IBaseUI.Init(Func<bool> _isShowFinishValid, Func<bool> _isHideFinishValid)
    {
        isShowFinishValid = _isShowFinishValid;
        isHideFinishValid = _isHideFinishValid;
        CacheTransition();
        if(localizationFileGroup != null)
        {
            await localizationFileGroup.LoadInfo();
        }
        Init();
    }

    void IBaseUI.Open(Action _showFinishCallBack)
    {
        showFinishCallBack_Anim = _showFinishCallBack;
        isShowFinished = false;
        isHideFinished = false;
        StartShow();
    }
    void IBaseUI.Close(Action _hideFinishCallBack)
    {
        hideFinishCallBack_Anim = _hideFinishCallBack;
        isShowFinished = false;
        isHideFinished = false;
        StartHide();
    }

    void IBaseUI.DestroyPanel()
    {
        DestroyPanel();
    }

    void IBaseUI.CompleteHide()
    {
        MoveToHideParent();
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
        PlayShowAnimation();
    }

    /// <summary>
    /// 准备隐藏
    /// </summary>
    protected virtual void StartHide()
    {
        PlayHideAnimation();
    }

    /// <summary>
    /// 打开完成
    /// </summary>
    public void ShowFinish()
    {
        if (isShowFinished)
        {
            return;
        }
        if (isShowFinishValid == null || isShowFinishValid())
        {
            isShowFinished = true;
            OnShowFinish();
        }
    }

    /// <summary>
    /// 打开完成
    /// </summary>
    protected virtual void OnShowFinish()
    {
        showFinishCallBack_Anim?.Invoke();
        showFinishCallBack_Anim = null;
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    public void HideFinish()
    {
        if (isHideFinished)
        {
            return;
        }
        if (isHideFinishValid == null || isHideFinishValid())
        {
            isHideFinished = true;
            OnHideFinish();
        }
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected virtual void OnHideFinish()
    {
        hideFinishCallBack_Anim?.Invoke();
        hideFinishCallBack_Anim = null;
    }

    /// <summary>
    /// 移动到指定父物体
    /// </summary>
    protected void MoveToParent(PanelManager panelManager, bool isShow)
    {
        if (isShow)
        {
            var parent = panelManager.Parent_2DUI;
            if (transform.parent != parent)
            {
                transform.SetParent(parent, false);
            }
        }
        else
        {
            var parent = panelManager.Parent_2DUI_Hide;
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

    protected abstract void MoveToShowParent();
    protected abstract void MoveToHideParent();

    protected T CreateModel<T>() where T : IBaseUI_Model, new()
    {
        var model = new T();
        model.Init();
        return model;
    }

    private void PlayShowAnimation()
    {
        if (uiTransition != null)
        {
            uiTransition.PlayShow(ShowFinish);
        }
        else
        {
            ShowFinish();
        }
    }

    private void PlayHideAnimation()
    {
        if (uiTransition != null)
        {
            uiTransition.PlayHide(HideFinish);
        }
        else
        {
            HideFinish();
        }
    }

    private void CacheTransition()
    {
        if (uiTransition == null)
        {
            TryGetComponent(out uiTransition);
        }
    }

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        TryGetComponent(out uiTransition);
    }
#endif
}
