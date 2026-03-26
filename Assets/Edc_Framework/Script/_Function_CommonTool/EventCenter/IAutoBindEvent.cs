using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoBindEventExtensions{
    /// <summary>
    /// 绑定事件（全局_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T>(this IAutoBindEvent bindEvent, EventDelegate action, GameObject bindObj){
        void addListener() { bindEvent.AddListener<T>(action); }
        void removeListener() { bindEvent.RemoveListener<T>(action); }
        bindEvent.SetBinder_DisableRemoveListenerr(addListener, removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（全局_带参数_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T>(this IAutoBindEvent bindEvent, EventDelegate<T> action, GameObject bindObj){
        void addListener() { bindEvent.AddListener(action); }
        void removeListener() { bindEvent.RemoveListener(action); }
        bindEvent.SetBinder_DisableRemoveListenerr(addListener, removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（全局_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T>(this IAutoBindEvent bindEvent, EventDelegate action, GameObject bindObj){
        bindEvent.AddListener<T>(action);
        void removeListener() { bindEvent.RemoveListener<T>(action); }
        bindEvent.SetBinder_DestroyRemoveListener(removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（全局_带参数_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T>(this IAutoBindEvent bindEvent, EventDelegate<T> action, GameObject bindObj){
        bindEvent.AddListener(action);
        void removeListener() { bindEvent.RemoveListener(action); }
        bindEvent.SetBinder_DestroyRemoveListener(removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }
}

public interface IAutoBindEvent : IBindEvent,IBaseAutoBindEvent{}
