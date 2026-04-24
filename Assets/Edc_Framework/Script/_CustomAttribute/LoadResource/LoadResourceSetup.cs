using System;

/// <summary>
/// 按照资源KEY加载
/// </summary>
public struct ResourceKey{
    public string Key { get; private set;}

    public ResourceKey(string key)
    {
        Key = key;
    }
}

/// <summary>
/// 按照资源Key加载
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ResourceKeyAttribute : Attribute
{
    public string Key { get; private set;}

    public ResourceKeyAttribute(string key)
    {
        Key = key;
    }
}

