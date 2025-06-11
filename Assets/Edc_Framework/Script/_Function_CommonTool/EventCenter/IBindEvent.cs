using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BindEventExtensions{
    /// <summary>
    /// 注册事件（全局）
    /// </summary>
    public static void AddListener<T>(this IBindEvent bindEvent, EventDelegate action){
        ((IEventCenterFunction)EventHub.Get()).AddListener<T>(action);
    }

    /// <summary>
    /// 注册事件（带参数）（全局）
    /// </summary>
    public static void AddListener<T>(this IBindEvent bindEvent, EventDelegate<T> action){
        ((IEventCenterFunction)EventHub.Get()).AddListener(action);
    }

    /// <summary>
    /// 移除事件（全局）
    /// </summary>
    public static void RemoveListener<T>(this IBindEvent bindEvent, EventDelegate action){
        ((IEventCenterFunction)EventHub.Get()).RemoveListener<T>(action);
    }

    /// <summary>
    /// 移除事件（带参数）（全局）
    /// </summary>
    public static void RemoveListener<T>(this IBindEvent bindEvent, EventDelegate<T> action){
        ((IEventCenterFunction)EventHub.Get()).RemoveListener(action);
    }

    /// <summary>
    /// 注册事件（独立）
    /// </summary>
    public static void AddListener<T, EventCenter>(this IBindEvent bindEvent, EventDelegate action)where EventCenter : IEventCenter
    {
        ((IEventCenterFunction)EventHub.Get<EventCenter>()).AddListener<T>(action);
    }

    /// <summary>
    /// 注册事件（带参数）（独立）
    /// </summary>
    public static void AddListener<T, EventCenter>(this IBindEvent bindEvent, EventDelegate<T> action)where EventCenter : IEventCenter
    {
        ((IEventCenterFunction)EventHub.Get<EventCenter>()).AddListener(action);
    }

    /// <summary>
    /// 移除事件（独立）
    /// </summary>
    public static void RemoveListener<T, EventCenter>(this IBindEvent bindEvent, EventDelegate action)where EventCenter : IEventCenter
    {
        ((IEventCenterFunction)EventHub.Get<EventCenter>()).RemoveListener<T>(action);
    }

    /// <summary>
    /// 移除事件（带参数）（独立）
    /// </summary>
    public static void RemoveListener<T, EventCenter>(this IBindEvent bindEvent, EventDelegate<T> action)where EventCenter : IEventCenter
    {
        ((IEventCenterFunction)EventHub.Get<EventCenter>()).RemoveListener(action);
    }
}

public interface IBindEvent{}
