using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventCenterLocalFunction
{
    void RegisterEventCenter(IEventCenterLocal eventCenter);
    void RemoveEventCenter<T>()where T : IEventCenterLocal;
    EventCenter GetValue<T>()where T : IEventCenterLocal;
}

public class EventCenterLocal : IEventCenterLocalFunction
{
    private readonly Dictionary<Type, EventCenter> indEventCenter = new();

    /// <summary>
    /// 注册局部事件中心
    /// </summary>
    void IEventCenterLocalFunction.RegisterEventCenter(IEventCenterLocal eventCenter){
        var type = eventCenter.GetType();
        if(indEventCenter.ContainsKey(type)){
            LogManager.LogError($"事件中心：{type.Name} 被重复注册");
        }
        else{
            indEventCenter.Add(type, eventCenter.EventCenter);
        }
    }

    /// <summary>
    /// 移除局部事件中心
    /// </summary>
    void IEventCenterLocalFunction.RemoveEventCenter<T>()
    {
        var type = typeof(T);
        if(indEventCenter.ContainsKey(type)){
            indEventCenter.Remove(type);
        }
        else{
            LogManager.LogError($"事件中心：{type.Name} 未注册或已删除");
        }
    }

    /// <summary>
    /// 获取局部事件
    /// </summary>
    EventCenter IEventCenterLocalFunction.GetValue<T>()
    {
        var type = typeof(T);
        if(indEventCenter.ContainsKey(type)){
            return indEventCenter[type];
        }
        return null;
    }
}
