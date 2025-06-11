using System;
using System.Collections.Generic;

public delegate void EventDelegate();
public delegate void EventDelegate<T>(T value);

public interface IEventCenterFunction{
    void AddListener<T>(EventDelegate action);
    void AddListener<T>(EventDelegate<T> action);
    void RemoveListener<T>(EventDelegate action);
    void RemoveListener<T>(EventDelegate<T> action);
    void SendEvent<T>();
    void SendEvent<T>(T value);
}

public class EventCenter : IEventCenterFunction
{
    public Dictionary<Type, Delegate> eventDelegates0;
    public Dictionary<Type, Delegate> eventDelegates1;

    public EventCenter(){
        eventDelegates0 = new Dictionary<Type, Delegate>();
        eventDelegates1 = new Dictionary<Type, Delegate>();
    }
#region 添加事件监听相关
    /// <summary>
    /// 添加事件监听
    /// </summary>
    void IEventCenterFunction.AddListener<T>(EventDelegate eventDelegate){
        var type = typeof(T);
        if(eventDelegates0.ContainsKey(type)){
            eventDelegates0[type] = ((EventDelegate)eventDelegates0[type]) + eventDelegate;
        }
        else{
            eventDelegates0.Add(type, eventDelegate);
        }
    }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    void IEventCenterFunction.AddListener<T>(EventDelegate<T> eventDelegate){
        var type = typeof(T);
        if(eventDelegates1.ContainsKey(type)){
            eventDelegates1[type] = ((EventDelegate<T>)eventDelegates1[type]) + eventDelegate;
        }
        else{
            eventDelegates1.Add(type, eventDelegate);
        }
    }
#endregion
#region 移除事件监听相关
    /// <summary>
    /// 移除事件监听
    /// </summary>
    void IEventCenterFunction.RemoveListener<T>(EventDelegate eventDelegate){
        var type = typeof(T);
        if(eventDelegates0.ContainsKey(type)){
            eventDelegates0[type] = ((EventDelegate)eventDelegates0[type]) - eventDelegate;
            if(eventDelegates0[type] == null){
                eventDelegates0.Remove(type);
            }
        }
        else{
            LogManager.LogError($"事件：{type.Name}没有添加委托");
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    void IEventCenterFunction.RemoveListener<T>(EventDelegate<T> eventDelegate){
        var type = typeof(T);
        if(eventDelegates1.ContainsKey(type)){
            eventDelegates1[type] = ((EventDelegate<T>)eventDelegates1[type]) - eventDelegate;
            if(eventDelegates1[type] == null)
                eventDelegates1.Remove(type);

        }
        else{
            LogManager.LogError($"事件：{type}没有添加委托");
        }
    }
#endregion
#region 触发事相关
    /// <summary>
    /// 触发事件
    /// </summary>
    void IEventCenterFunction.SendEvent<T>(){
        var type = typeof(T);
        if(eventDelegates0.ContainsKey(type)){
            ((EventDelegate)eventDelegates0[type]).Invoke();
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    void IEventCenterFunction.SendEvent<T>(T value1){
        var type = typeof(T);
        if(eventDelegates1.ContainsKey(type)){
            ((EventDelegate<T>)eventDelegates1[type]).Invoke(value1);
        }
    }
#endregion
}
