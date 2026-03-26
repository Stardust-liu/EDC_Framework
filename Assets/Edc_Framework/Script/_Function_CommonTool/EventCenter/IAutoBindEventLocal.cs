using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoBindEventLocalExtensions
{
    /// <summary>
    /// 绑定事件（独立_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T, TEventCenter>(this IAutoBindEventLocal bindEvent, EventDelegate action, GameObject bindObj)where TEventCenter : IEventCenterLocal
    {
        void addListener() { bindEvent.AddListener<T, TEventCenter>(action); }
        void removeListener() { bindEvent.RemoveListener<T, TEventCenter>(action); }
        bindEvent.SetBinder_DisableRemoveListenerr(addListener, removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_带参数_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T, TEventCenter>(this IAutoBindEventLocal bindEvent, EventDelegate<T> action, GameObject bindObj)where TEventCenter : IEventCenterLocal
    {
        void addListener() { bindEvent.AddListener<T, TEventCenter>(action); }
        void removeListener() { bindEvent.RemoveListener<T, TEventCenter>(action); }
        bindEvent.SetBinder_DisableRemoveListenerr(addListener, removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T, TEventCenter>(this IAutoBindEventLocal bindEvent, EventDelegate action, GameObject bindObj)where TEventCenter : IEventCenterLocal
    {
        bindEvent.AddListener<T, TEventCenter>(action);
        void removeListener() { bindEvent.RemoveListener<T, TEventCenter>(action); }
        bindEvent.SetBinder_DestroyRemoveListener(removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_带参数_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T, TEventCenter>(this IAutoBindEventLocal bindEvent, EventDelegate<T> action, GameObject bindObj)where TEventCenter : IEventCenterLocal
    {
        bindEvent.AddListener<T, TEventCenter>(action);
        void removeListener() { bindEvent.RemoveListener<T, TEventCenter>(action); }
        bindEvent.SetBinder_DestroyRemoveListener(removeListener, bindEvent.GetEventAutoBinder(bindObj));
    }
}
public interface IAutoBindEventLocal : IBindEventLocal, IBaseAutoBindEvent{}
