using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoBindEventExtensions{
    /// <summary>
    /// 绑定事件（全局_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T>(this IAutoBindEvent bindEvent, EventDelegate action, GameObject bindObj){
        Action addListener = ()=>{bindEvent.AddListener<T>(action);};
        Action removeListener = ()=>{bindEvent.RemoveListener<T>(action);};
        SetBinder_DisableRemoveListenerr(addListener, removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（全局_带参数_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T>(this IAutoBindEvent bindEvent, EventDelegate<T> action, GameObject bindObj){
        Action addListener = ()=>{bindEvent.AddListener(action);};
        Action removeListener = ()=>{bindEvent.RemoveListener(action);};
        SetBinder_DisableRemoveListenerr(addListener, removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（全局_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T>(this IAutoBindEvent bindEvent, EventDelegate action, GameObject bindObj){
        bindEvent.AddListener<T>(action);
        Action removeListener = ()=>{bindEvent.RemoveListener<T>(action);};
        SetBinder_DestroyRemoveListener(removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（全局_带参数_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T>(this IAutoBindEvent bindEvent, EventDelegate<T> action, GameObject bindObj){
        bindEvent.AddListener(action);
        Action removeListener = ()=>{bindEvent.RemoveListener(action);};
        SetBinder_DestroyRemoveListener(removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T, EventCenter>(this IAutoBindEvent bindEvent, EventDelegate action, GameObject bindObj)where EventCenter : IEventCenter
    {
        Action addListener = ()=>{bindEvent.AddListener<T, EventCenter>(action);};
        Action removeListener = ()=>{bindEvent.RemoveListener<T>(action);};
        SetBinder_DisableRemoveListenerr(addListener, removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_带参数_Disable时移除）
    /// </summary>
    public static void AddListener_EnableDisable<T, EventCenter>(this IAutoBindEvent bindEvent, EventDelegate<T> action, GameObject bindObj)where EventCenter : IEventCenter
    {
        Action addListener = ()=>{bindEvent.AddListener<T, EventCenter>(action);};
        Action removeListener = ()=>{bindEvent.RemoveListener<T, EventCenter>(action);};
        SetBinder_DisableRemoveListenerr(addListener, removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T, EventCenter>(this IAutoBindEvent bindEvent, EventDelegate action, GameObject bindObj)where EventCenter : IEventCenter
    {
        bindEvent.AddListener<T, EventCenter>(action);
        Action removeListener = ()=>{bindEvent.RemoveListener<T, EventCenter>(action);};
        SetBinder_DestroyRemoveListener(removeListener, GetEventAutoBinder(bindObj));
    }

    /// <summary>
    /// 绑定事件（独立_带参数_Destroy时移除）
    /// </summary>
    public static void AddListener_StartDestroy<T, EventCenter>(this IAutoBindEvent bindEvent, EventDelegate<T> action, GameObject bindObj)where EventCenter : IEventCenter
    {
        bindEvent.AddListener<T, EventCenter>(action);
        Action removeListener = ()=>{bindEvent.RemoveListener<T, EventCenter>(action);};
        SetBinder_DestroyRemoveListener(removeListener, GetEventAutoBinder(bindObj));
    }


    private static void SetBinder_DisableRemoveListenerr(Action addListener, Action removeListener, EventAutoBinder eventAutoBinder){
        eventAutoBinder.RegisterEnableAddListener(addListener);
        eventAutoBinder.RegisterDisableRemoveListener(removeListener);
    }

    private static void SetBinder_DestroyRemoveListener(Action removeListener, EventAutoBinder eventAutoBinder){
        eventAutoBinder.RegisterDestroyRemoveListener(removeListener);
    }

    private static EventAutoBinder GetEventAutoBinder(GameObject bindObj){
        if(!bindObj.TryGetComponent<EventAutoBinder>(out var eventAutoBinder))
        {
            eventAutoBinder = bindObj.AddComponent<EventAutoBinder>();
        }
        return eventAutoBinder;
    }
}

public interface IAutoBindEvent : IBindEvent{}
