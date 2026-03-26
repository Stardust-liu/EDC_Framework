using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BindEventExtensions{
    /// <summary>
    /// 注册事件（全局）
    /// </summary>
    public static void AddListener<T>(this IBindEvent bindEvent, EventDelegate action){
        ((IEventCenterFunction)EventHub.Global).AddListener<T>(action);
    }

    /// <summary>
    /// 移除事件（全局）
    /// </summary>
    public static void RemoveListener<T>(this IBindEvent bindEvent, EventDelegate action){
        ((IEventCenterFunction)EventHub.Global).RemoveListener<T>(action);
    }

    /// <summary>
    /// 注册事件（带参数）（全局）
    /// </summary>
    public static void AddListener<T>(this IBindEvent bindEvent, EventDelegate<T> action){
        ((IEventCenterFunction)EventHub.Global).AddListener(action);
    }

    /// <summary>
    /// 移除事件（带参数）（全局）
    /// </summary>
    public static void RemoveListener<T>(this IBindEvent bindEvent, EventDelegate<T> action){
        ((IEventCenterFunction)EventHub.Global).RemoveListener(action);
    }
}

public interface IBindEvent{}
