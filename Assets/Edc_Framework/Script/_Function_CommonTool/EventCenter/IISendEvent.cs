using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SendEventExtensions{
    /// <summary>
    /// 发送事件（全局）
    /// </summary>
    public static void SendEvent<T>(this ISendEvent sendEvent){
        ((IEventCenterFunction)EventHub.Get()).SendEvent<T>();
    }

    /// <summary>
    /// 发送事件（全局）
    /// </summary>
    public static void SendEvent<T>(this ISendEvent sendEvent, T value){
        ((IEventCenterFunction)EventHub.Get()).SendEvent(value);
    }

    /// <summary>
    /// 发送事件（独立）
    /// </summary>
    public static void SendEvent<T, EventCenter>(this ISendEvent sendEvent) where EventCenter : IEventCenter
    {
        var eventCenter = (IEventCenterFunction)EventHub.Get<EventCenter>();
        if (eventCenter != null)
        {
            ((IEventCenterFunction)EventHub.Get<EventCenter>()).SendEvent<T>();
        }
        else
        {
            LogManager.LogWarning($"事件中心{typeof(EventCenter).Name}未添加，或是已被移除");
        }
    }

    /// <summary>
    /// 发送事件（独立）
    /// </summary>
    public static void SendEvent<T, EventCenter>(this ISendEvent sendEvent, T value)where EventCenter : IEventCenter
    {
        var eventCenter = (IEventCenterFunction)EventHub.Get<EventCenter>();
        if (eventCenter != null)
        {
            ((IEventCenterFunction)EventHub.Get<EventCenter>()).SendEvent(value);
        }
        else
        {
            LogManager.LogWarning($"事件中心{typeof(EventCenter).Name}未添加，或是已被移除");
        }
    }
}

public interface ISendEvent{}
