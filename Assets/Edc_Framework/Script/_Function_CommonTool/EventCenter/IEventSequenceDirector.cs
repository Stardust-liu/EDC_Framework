using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventSequenceDirectorExtensions
{
    /// <summary>
    /// 添加序列事件
    /// </summary>
    public static void AddSequence<SequenceName, EventName>(this IEventSequenceDirector sequenceDirector){
        ((IEventSequenceFunction)EventHub.Sequence).AddSequence<SequenceName, EventName>();
    }

    /// <summary>
    /// 移除序列事件
    /// </summary>
    public static void RemoveSequence<SequenceName, EventName>(this IEventSequenceDirector sequenceDirector){
        ((IEventSequenceFunction)EventHub.Sequence).RemoveSequence<SequenceName, EventName>();
    }

    /// <summary>
    /// 设置序列事件
    /// </summary>
    public static void SetSequence<SequenceName>(this IEventSequenceDirector sequenceDirector, List<Type> types){
        ((IEventSequenceFunction)EventHub.Sequence).SetSequence<SequenceName>(types);
    }
}

public interface IEventSequenceDirector{}