using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BindEventSequenceExtensions
{
    /// <summary>
    /// 添加序列事件监听
    /// </summary>
    public static void AddListener<SequenceName, EventName>(this IBindEventSequence bindEvent, EventDelegate_Task action){
        ((IEventSequenceFunction)EventHub.Sequence).AddListener<SequenceName, EventName>(action);
    }

    /// <summary>
    /// 移除序列事件监听
    /// </summary>
    public static void RemoveListener<SequenceName, EventName>(this IBindEventSequence bindEvent, EventDelegate_Task action){
        ((IEventSequenceFunction)EventHub.Sequence).RemoveListener<SequenceName, EventName>(action);
    }
}
public interface IBindEventSequence {}
