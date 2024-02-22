using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EventCenter
{
    public delegate void EventDelegate();
    public delegate void EventDelegate<T>(T value);
    public delegate void EventDelegate<T1,T2>(T1 value1, T2 value2);
    public delegate void EventDelegate<T1,T2,T3>(T1 value1, T2 value2, T3 value3);

    public Dictionary<string, Delegate> eventDelegates0;
    public Dictionary<string, Delegate> eventDelegates1;
    public Dictionary<string, Delegate> eventDelegates2;
    public Dictionary<string, Delegate> eventDelegates3;

    public EventCenter(){
        eventDelegates0 = new Dictionary<string, Delegate>();
        eventDelegates1 = new Dictionary<string, Delegate>();
        eventDelegates2 = new Dictionary<string, Delegate>();
        eventDelegates3 = new Dictionary<string, Delegate>();
    }
#region 添加事件监听相关
    /// <summary>
    /// 添加事件监听
    /// </summary>
    public void AddListener(string eventName ,EventDelegate eventDelegate){
        if(eventDelegates0.ContainsKey(eventName)){
            eventDelegates0[eventName] = ((EventDelegate)eventDelegates0[eventName]) + eventDelegate;
        }
        else{
            eventDelegates0.Add(eventName, eventDelegate);
        }
    }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    public void AddListener<T>(string eventName ,EventDelegate<T> eventDelegate){
        if(eventDelegates1.ContainsKey(eventName)){
            eventDelegates1[eventName] = ((EventDelegate<T>)eventDelegates1[eventName]) + eventDelegate;
        }
        else{
            eventDelegates1.Add(eventName, eventDelegate);
        }
    }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    public void AddListener<T1, T2>(string eventName ,EventDelegate<T1, T2> eventDelegate){
        if(eventDelegates2.ContainsKey(eventName)){
            eventDelegates2[eventName] = ((EventDelegate<T1, T2>)eventDelegates2[eventName]) + eventDelegate;
        }
        else{
            eventDelegates2.Add(eventName, eventDelegate);
        }
    }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    public void AddListener<T1, T2, T3>(string eventName ,EventDelegate<T1, T2, T3> eventDelegate){
        if(eventDelegates3.ContainsKey(eventName)){
            eventDelegates3[eventName] = ((EventDelegate<T1, T2, T3>)eventDelegates3[eventName]) + eventDelegate;
        }
        else{
            eventDelegates3.Add(eventName, eventDelegate);
        }
    }
#endregion
#region 移除事件监听相关
    /// <summary>
    /// 移除事件监听
    /// </summary>
    public void RemoveListener(string eventName, EventDelegate eventDelegate){
        if(eventDelegates0.ContainsKey(eventName)){
            eventDelegates0[eventName] = ((EventDelegate)eventDelegates0[eventName]) - eventDelegate;
            if(eventDelegates0[eventName] == null)
                eventDelegates0.Remove(eventName);
        }
        else{
            LogManager.LogError($"事件：{eventName}没有添加委托");
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    public void RemoveListener<T>(string eventName, EventDelegate<T> eventDelegate){
        if(eventDelegates1.ContainsKey(eventName)){
            eventDelegates1[eventName] = ((EventDelegate<T>)eventDelegates1[eventName]) - eventDelegate;
            if(eventDelegates1[eventName] == null)
                eventDelegates1.Remove(eventName);

        }
        else{
            LogManager.LogError($"事件：{eventName}没有添加委托");
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    public void RemoveListener<T1, T2>(string eventName, EventDelegate<T1, T2> eventDelegate){
        if(eventDelegates2.ContainsKey(eventName)){
            eventDelegates2[eventName] = ((EventDelegate<T1, T2>)eventDelegates2[eventName]) - eventDelegate;
            if(eventDelegates2[eventName] == null)
                eventDelegates2.Remove(eventName);
        }
        else{
            LogManager.LogError($"事件：{eventName}没有添加委托");
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    public void RemoveListener<T1, T2, T3>(string eventName, EventDelegate<T1, T2, T3> eventDelegate){
        if(eventDelegates3.ContainsKey(eventName)){
            eventDelegates3[eventName] = ((EventDelegate<T1, T2, T3>)eventDelegates3[eventName]) - eventDelegate;
            if(eventDelegates3[eventName] == null)
                eventDelegates3.Remove(eventName);
        }
        else{
            LogManager.LogError($"事件：{eventName}没有添加委托");
        }
    }
#endregion
#region 触发事相关
    /// <summary>
    /// 触发事件
    /// </summary>
    public void OnEvent(string eventName){
        if(eventDelegates0.ContainsKey(eventName)){
            ((EventDelegate)eventDelegates0[eventName])();
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void OnEvent<T>(string eventName, T value1){
        if(eventDelegates1.ContainsKey(eventName)){
            ((EventDelegate<T>)eventDelegates1[eventName])(value1);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void OnEvent<T1, T2>(string eventName, T1 value1, T2 value2){
        if(eventDelegates2.ContainsKey(eventName)){
            ((EventDelegate<T1, T2>)eventDelegates2[eventName])(value1, value2);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void OnEvent<T1, T2, T3>(string eventName, T1 value1, T2 value2, T3 value3){
        if(eventDelegates3.ContainsKey(eventName)){
            ((EventDelegate<T1, T2, T3>)eventDelegates3[eventName])(value1, value2, value3);
        }
    }
#endregion
}
