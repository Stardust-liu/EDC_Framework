using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BindEventLocalExtensions
{
   /// <summary>
    /// 注册事件（局部）
    /// </summary>
    public static void AddListener<T, TEventCenter>(this IBindEventLocal bindEvent, EventDelegate action)where TEventCenter : IEventCenterLocal
    {
        var eventCenter = ((IEventCenterLocalFunction)EventHub.Local).GetValue<TEventCenter>();
        ((IEventCenterFunction)eventCenter).AddListener<T>(action);
    }

    /// <summary>
    /// 移除事件（局部）
    /// </summary>
    public static void RemoveListener<T, TEventCenter>(this IBindEventLocal bindEvent, EventDelegate action)where TEventCenter : IEventCenterLocal
    {
        var eventCenter = ((IEventCenterLocalFunction)EventHub.Local).GetValue<TEventCenter>();
        ((IEventCenterFunction)eventCenter).RemoveListener<T>(action);
    }

    /// <summary>
    /// 注册事件（带参数）（局部）
    /// </summary>
    public static void AddListener<T, TEventCenter>(this IBindEventLocal bindEvent, EventDelegate<T> action)where TEventCenter : IEventCenterLocal
    {
        var eventCenter = ((IEventCenterLocalFunction)EventHub.Local).GetValue<TEventCenter>();
        ((IEventCenterFunction)eventCenter).AddListener(action);
    }

    /// <summary>
    /// 移除事件（带参数）（局部）
    /// </summary>
    public static void RemoveListener<T, TEventCenter>(this IBindEventLocal bindEvent, EventDelegate<T> action)where TEventCenter : IEventCenterLocal
    {
        var eventCenter = ((IEventCenterLocalFunction)EventHub.Local).GetValue<TEventCenter>();
        ((IEventCenterFunction)eventCenter).RemoveListener(action);
    }
}

public interface IBindEventLocal{}
