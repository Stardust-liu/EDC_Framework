using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;


public delegate UniTask EventDelegate_Task();

interface IEventSequenceFunction
{
    void AddListener<SequenceName, EventName>(EventDelegate_Task eventDelegate);
    void RemoveListener<SequenceName, EventName>(EventDelegate_Task eventDelegate);
    void AddSequence<SequenceName, EventName>();
    void RemoveSequence<SequenceName, EventName>();
    void SetSequence<SequenceName>(List<Type> types);
    UniTask SendEventSequenceAsync<SequenceName>();
    void SendEventSequence<SequenceName>();
}

public class EventSequence : IEventSequenceFunction
{
    private Dictionary<Type, List<Type>> keyValuePairs = new();
    private Dictionary<Type, Delegate> eventDelegates = new();


    public EventSequence(){
        keyValuePairs = new Dictionary<Type, List<Type>>();
        eventDelegates = new Dictionary<Type, Delegate>();
    }


    /// <summary>
    /// 添加序列监听
    /// </summary>
    void IEventSequenceFunction.AddListener<SequenceName, EventName>(EventDelegate_Task eventDelegate)
    {
        var sequenceName = typeof(SequenceName);
        var eventName = typeof(EventName);

        if (!keyValuePairs.ContainsKey(sequenceName))
        {
            var queue = new List<Type>();
            keyValuePairs.Add(sequenceName, queue);
        }
        
        if(eventDelegates.ContainsKey(eventName)){
            eventDelegates[eventName] = ((EventDelegate_Task)eventDelegates[eventName]) + eventDelegate;
        }
        else{
            eventDelegates.Add(eventName, eventDelegate);
        }
    }

    /// <summary>
    /// 移除序列监听
    /// </summary>
    void IEventSequenceFunction.RemoveListener<SequenceName, EventName>(EventDelegate_Task eventDelegate)
    {
        var sequenceName = typeof(SequenceName);
        var eventName = typeof(EventName);

        if (!keyValuePairs.ContainsKey(sequenceName))
        {
            LogManager.LogError($"事件：{eventName.Name}还没添加到{sequenceName.Name}序列就被移除");
        }
        
        if(eventDelegates.ContainsKey(eventName)){
            eventDelegates[eventName] = ((EventDelegate_Task)eventDelegates[eventName]) - eventDelegate;
            if (eventDelegates[eventName] == null)
            {
                eventDelegates.Remove(eventName);
            }
        }
        else{
            LogManager.LogError($"事件：{eventName.Name}没添加任何委托就被移除");
        }
    }

    /// <summary>
    /// 添加序列事件
    /// </summary>
    void IEventSequenceFunction.AddSequence<SequenceName, EventName>()
    {
        var sequenceName = typeof(SequenceName);
        var eventName = typeof(EventName);

        if (!keyValuePairs.ContainsKey(sequenceName))
        {
            keyValuePairs.Add(sequenceName, new List<Type>(){eventName});
        }
        else
        {
            keyValuePairs[sequenceName].Add(eventName);
        }
    }

    /// <summary>
    /// 移除序列事件
    /// </summary>
    void IEventSequenceFunction.RemoveSequence<SequenceName, EventName>()
    {
        var sequenceName = typeof(SequenceName);
        var eventName = typeof(EventName);

        if (keyValuePairs.TryGetValue(sequenceName, out var list))
        {
            if (list.Remove(eventName))
            {
                if (list.Count == 0)
                {
                    keyValuePairs.Remove(sequenceName);
                }
            }
            else
            {
                LogManager.LogWarning($"序列{sequenceName}中没有{eventName}事件");
            }
        }
        else
        {
            LogManager.LogWarning($"还没有添加{sequenceName}序列");
        }
    }

    /// <summary>
    /// 设置序列
    /// </summary>
    public void SetSequence<SequenceName>(List<Type> types)
    {
        var sequenceName = typeof(SequenceName);
        if (!keyValuePairs.ContainsKey(sequenceName))
        {
            keyValuePairs.Add(sequenceName, types);
        }
        else
        {
            keyValuePairs[sequenceName] = types;
        }
    }

    /// <summary>
    /// 触发事件序列
    /// </summary>
    async UniTask IEventSequenceFunction.SendEventSequenceAsync<SequenceName>()
    {
        var sequenceName = typeof(SequenceName);
        if (keyValuePairs.ContainsKey(sequenceName))
        {
            var list = keyValuePairs[sequenceName];
            foreach (var item in list)
            {
                if(eventDelegates.TryGetValue(item, out var del)){
                    var delList = del.GetInvocationList();
                    var delCount = delList.Length;
                    var eventDelegate_Tasks = new UniTask[delCount];

                    for (int i = 0; i < delCount; i++)
                    {
                        var aitem = (EventDelegate_Task)delList[i];
                        eventDelegate_Tasks[i] = aitem();
                    }
                    await UniTask.WhenAll(eventDelegate_Tasks);
                }
            }
        }
    }

    /// <summary>
    /// 触发事件序列(无等待)
    /// </summary>
    public void SendEventSequence<SequenceName>()
    {
        var sequenceName = typeof(SequenceName);
        if (keyValuePairs.ContainsKey(sequenceName))
        {
            var list = keyValuePairs[sequenceName];
            foreach (var item in list)
            {
                if(eventDelegates.TryGetValue(item, out var del)){
                    var delList = del.GetInvocationList();
                    var delCount = delList.Length;
                    for (int i = 0; i < delCount; i++)
                    {
                        ((EventDelegate_Task)delList[i]).Invoke();
                    }
                }
            }
        }
    }
}
