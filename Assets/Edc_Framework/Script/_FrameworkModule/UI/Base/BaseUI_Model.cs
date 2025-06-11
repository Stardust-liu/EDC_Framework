using System;
using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using UnityEngine;

public interface IBaseUI_Model{
    void Init();
}

public abstract class BaseUI_Model : IBaseUI_Model
{
    private Dictionary<Type, BaseIOCComponent> component = new ();
    private Dictionary<Type, BaseMonoIOCComponent> monoComponent = new ();
    
    void IBaseUI_Model.Init(){
        Init();
    }

    protected abstract void Init();

    /// <summary>
    /// 注册会用到的IOC模块
    /// </summary>
    protected void RegisterData<T>(BaseIOCComponent baseIOCComponent)
    {
        var type = typeof(T);
        if(!component.ContainsKey(type))
        {
            component[type] = baseIOCComponent;
        }
    }

    /// <summary>
    /// 注册会用到的IOC模块
    /// </summary>
    protected void RegisterData<T>(BaseMonoIOCComponent baseMonoIOCComponent)
    {
        var type = typeof(T);
        if(!monoComponent.ContainsKey(type))
        {
            monoComponent[type] = baseMonoIOCComponent;
        }
    }

    /// <summary>
    /// 获取IOC模块
    /// </summary>
    protected T Get<T>() where T : class
    {
        var type = typeof(T);
        if(component.TryGetValue(type, out var value1)){
            return value1 as T;
        }
        if(monoComponent.TryGetValue(type, out var value2)){
            return value2 as T;
        }
        return null;
    }
}
