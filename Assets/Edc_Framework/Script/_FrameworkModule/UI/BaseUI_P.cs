using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI_P
{
    protected bool isHideFinishDestroy;
    protected bool isCreate;
    protected bool isShowFinish;
    protected bool isCloseFinish;
    private bool isShow;
    public bool IsCreate{get{return isCreate;}}
    public bool IsShow{get{return isShow;}}
    public bool IsShowFinish{get{return isShowFinish;}}
    public bool IsCloseFinish{get{return isCloseFinish;}}

    public abstract void CreateUiPrefab();
    protected abstract void CreateUiPrefab(string uiName);
    protected abstract GameObject SetPrefabInfo(string uiName);

    /// <summary>
    /// 准备打开
    /// </summary>
    protected virtual void StartShow(){
        isShow = true;
        isCloseFinish = false;
    }

    /// <summary>
    /// 准备隐藏
    /// </summary>
    protected virtual void StartHide(){
        isShow = false;
        isShowFinish = false;
    }

    /// <summary>
    /// 打开完成
    /// </summary>
    protected virtual void ShowFinish(){
        isShowFinish = true;
    }

    /// <summary>
    /// 隐藏完成
    /// </summary>
    protected virtual void HideFinish(){
        isCloseFinish = true;
        if (isHideFinishDestroy){
            Destroy();
        }
    }

    /// <summary>
    /// 销毁界面
    /// </summary>
    public virtual void Destroy(){
        isCreate = false;
    }
}
