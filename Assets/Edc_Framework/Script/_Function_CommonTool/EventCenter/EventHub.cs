using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EventHub
{
    /// <summary>
    /// 获取事件序列
    /// </summary>
    public static EventSequence Sequence {get{return sequence;}}

    /// <summary>
    /// 获取全局事件中心
    /// </summary>
    public static EventCenter Global {get{return global;}}

    /// <summary>
    /// 获取局部事件中心（作用域隔离）
    /// </summary>
    public static EventCenterLocal Local {get{return local;}}

    private static readonly EventSequence sequence = new();
    private static readonly EventCenter global = new();
    private static readonly EventCenterLocal local = new();
}
