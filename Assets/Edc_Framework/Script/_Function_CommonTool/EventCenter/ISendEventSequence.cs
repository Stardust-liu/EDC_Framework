using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class SendEventSequenceExtensions
{
    /// <summary>
    /// 触发事件序列
    /// </summary>
    public static async UniTask SendEventSequenceAsync<SequenceName>(this ISendEventSequence sendEvent){
        await ((IEventSequenceFunction)EventHub.Sequence).SendEventSequenceAsync<SequenceName>();
    }

    /// <summary>
    /// 触发事件序列（无等待）
    /// </summary>
    public static void SendEventSequence<SequenceName>(this ISendEventSequence sendEvent){
        ((IEventSequenceFunction)EventHub.Sequence).SendEventSequence<SequenceName>();
    }
}

public interface ISendEventSequence {}
