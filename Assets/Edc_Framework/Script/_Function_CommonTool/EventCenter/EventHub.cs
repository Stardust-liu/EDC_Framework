using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHub
{
    private static EventHub instance;
    private static EventHub Instance{
        get{
            if (instance == null)
            {
                instance = new();
                instance.globalEventCenter = new();
                instance.indEventCenter = new();
            }
            return instance;
        }
    }
    private const string dataParentFolder = "Data";
    private EventCenter globalEventCenter;
    private Dictionary<Type, EventCenter> indEventCenter;


    /// <summary>
    /// 注冊事件中心
    /// </summary>
    public static void Register(IEventCenter eventCenter){
        Instance.RegisterValue(eventCenter);
    }

    /// <summary>
    /// 移除事件中心
    /// </summary>
    public static void Remove<T>() where T : IEventCenter
    {
        Instance.RemoveValue<T>();
    }

    /// <summary>
    /// 获取独立事件中心
    /// </summary>
    public static EventCenter Get<T>() where T : IEventCenter
    {
        return Instance.GetValue<T>();
    }

    /// <summary>
    /// 获取全局事件中心
    /// </summary>
    public static EventCenter Get()
    {
        return Instance.globalEventCenter;
    }

    private void RegisterValue(IEventCenter eventCenter){
        var type = eventCenter.GetType();
        if(indEventCenter.ContainsKey(type)){
            LogManager.LogError($"事件中心：{type.Name} 被重复注册");
        }
        else{
            indEventCenter.Add(type, eventCenter.EventCenter);
        }
    }

    private void RemoveValue<T>()where T : IEventCenter
    {
        var type = typeof(T);
        if(indEventCenter.ContainsKey(type)){
            indEventCenter.Remove(type);
        }
        else{
            LogManager.LogError($"事件中心：{type.Name} 未注册或已删除");
        }
    }

    private EventCenter GetValue<T>()where T : IEventCenter
    {
        var type = typeof(T);
        if(indEventCenter.ContainsKey(type)){
            return indEventCenter[type];
        }
        return null;
    }
}
