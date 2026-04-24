using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
public interface IContainer{
    //为了方便调用Register后，可以直接调用模块中的异步方法，因此使用了通过out赋值，同时返回实例对象的方法
    T Register<T>(out T value) where T : class, IIOCComponent;
    T Register<T>(T instance, out T value) where T : class, IIOCComponent; 
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
    public static async UniTask Init()
    {
        await Instance.InitContainer();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public static async UniTask Init(FrameworkManager frameworkManager)
    {
        await Instance.InitContainer(frameworkManager);
    }

    protected virtual UniTask InitContainer(){return UniTask.CompletedTask;}
    protected virtual UniTask InitContainer(FrameworkManager frameworkManager){return UniTask.CompletedTask;}

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
    T IContainer.Register<T>(out T value)
    {
        var type = typeof(T);
        if(iocDictionary.ContainsKey(type)){
            LogManager.LogError($"对象：{type.Name} 被重复注册");
        }
        var instance = Activator.CreateInstance<T>();
        value = instance;
        instance.Init();
        iocDictionary.Add(type, instance);
        return value;
    }

    /// <summary>
    /// 注册
    /// </summary>
    T IContainer.Register<T>(T instance, out T value)
    {
        var type = typeof(T);
        if(iocDictionary.ContainsKey(type)){
            LogManager.LogError($"对象：{type.Name} 被重复注册");
        }
        value = instance;
        instance.Init();
        iocDictionary.Add(type, instance);
        return value;
    }

    /// <summary>
    /// 移除
    /// </summary>
    void IContainer.Unregister<T>()
    { 
        Instance.UnregisterValue<T>();
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
