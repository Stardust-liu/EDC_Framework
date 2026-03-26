using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SendEventExtensions{
    /// <summary>
    /// 发送事件（全局）
    /// </summary>
    public static void SendEvent<T>(this ISendEvent sendEvent){
        ((IEventCenterFunction)EventHub.Global).SendEvent<T>();
    }

    /// <summary>
    /// 发送事件（全局）
    /// </summary>
    public static void SendEvent<T>(this ISendEvent sendEvent, T value){
        ((IEventCenterFunction)EventHub.Global).SendEvent(value);
    }
}

public interface ISendEvent{}
