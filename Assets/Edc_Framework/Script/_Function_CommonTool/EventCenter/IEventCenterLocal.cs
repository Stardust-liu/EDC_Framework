using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCenterLocalExtensions
{
    /// <summary>
    /// 注册局部事件中心
    /// </summary>
    public static void Register(this IEventCenterLocal eventCenterLocal, IEventCenterLocal eventCenter){
        ((IEventCenterLocalFunction)EventHub.Local).RegisterEventCenter(eventCenter);
    }

    /// <summary>
    /// 移除局部事件中心
    /// </summary>
    public static void Remove<TEventCenter>(this IEventCenterLocal eventCenterLocal) where TEventCenter : IEventCenterLocal
    {
        ((IEventCenterLocalFunction)EventHub.Local).RemoveEventCenter<TEventCenter>();
    }
}

public interface IEventCenterLocal
{
    public EventCenter EventCenter{get; set;}
}
