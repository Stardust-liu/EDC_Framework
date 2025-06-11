using System;
using System.Collections.Generic;
public interface IContainer{
    T Register<T>() where T : class, IIOCComponent; 
    T Register<T>(T instance) where T : class, IIOCComponent; 
    void Unregister<T>() where T : class, IIOCComponent; 
}
public abstract class IOCContainer<TSingle> : IContainer
where TSingle : IOCContainer<TSingle>, new()
{
    private static TSingle instance;
    public static TSingle Instance{
        get {
            instance ??= new TSingle();
            return instance;
        }
    }

    private readonly Dictionary<Type, object> iocDictionary = new();

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        Instance.InitContainer();
    }

    /// <summary>
    /// 获取
    /// </summary>
    public static T Get<T>()where T : class, IIOCComponent
    { 
        return Instance.GetValue<T>();
    }

    /// <summary>
    /// 注册
    /// </summary>
    T IContainer.Register<T>()
    { 
        return Instance.RegisterValue<T>();
    }

    /// <summary>
    /// 注册
    /// </summary>
    T IContainer.Register<T>(T instance)
    { 
        return Instance.RegisterValue<T>(instance);
    }

    /// <summary>
    /// 移除
    /// </summary>
    void IContainer.Unregister<T>()
    { 
        Instance.UnregisterValue<T>();
    }

    protected abstract void InitContainer();

    private T RegisterValue<T>()where T : class, IIOCComponent
    {
        var type = typeof(T);
        if(iocDictionary.ContainsKey(type)){
            LogManager.LogError($"对象：{type.Name} 被重复注册");
        }
        else{
            var instance = Activator.CreateInstance<T>();
            instance.Init();
            iocDictionary.Add(type, instance);
        }
        return (T)iocDictionary[type];
    }

    private T RegisterValue<T>(T instance)where T : class, IIOCComponent
    {
        var type = typeof(T);
        if(iocDictionary.TryGetValue(type, out var value)){
            LogManager.LogError($"对象：{type.Name} 被重复注册");
        }
        else{
            instance.Init();
            iocDictionary.Add(type, instance);
        }
        return (T)iocDictionary[type];
    }

    private void UnregisterValue<T>()where T : class, IIOCComponent
    {
        var type = typeof(T);
        if(iocDictionary.TryGetValue(type, out var value)){
            ((T)value).Uninstall();
            iocDictionary.Remove(type);
        }
        else{
            LogManager.LogWarning($"对象：{type.Name} 未被注册");
        }
    }

    private T GetValue<T>() where T : class, IIOCComponent
    {
        var type = typeof(T);
        if(iocDictionary.TryGetValue(type, out var value)){
            return (T)value;
        }
        return null;
    }
}
