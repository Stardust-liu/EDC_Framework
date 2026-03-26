using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SendEventLocalExtensions
{
    /// <summary>
    /// 发送事件（局部）
    /// </summary>
    public static void SendEvent<T, TEventCenter>(this ISendEventLocal sendEvent) where TEventCenter : IEventCenterLocal
    {
        var eventCenter = ((IEventCenterLocalFunction)EventHub.Local).GetValue<TEventCenter>();
        if (eventCenter != null)
        {
            ((IEventCenterFunction)eventCenter).SendEvent<T>();
        }
        else
        {
            LogManager.LogWarning($"事件中心{typeof(EventCenter).Name}未添加，或是已被移除");
        }
    }

    /// <summary>
    /// 发送事件（局部）
    /// </summary>
    public static void SendEvent<T, TEventCenter>(this ISendEventLocal sendEvent, T value)where TEventCenter : IEventCenterLocal
    {
        var eventCenter = ((IEventCenterLocalFunction)EventHub.Local).GetValue<TEventCenter>();
        if (eventCenter != null)
        {
            ((IEventCenterFunction)eventCenter).SendEvent(value);
        }
        else
        {
            LogManager.LogWarning($"事件中心{typeof(TEventCenter).Name}未添加，或是已被移除");
            
        }
    }
}
public interface ISendEventLocal{}

