using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按照资源路径加载
/// </summary>
public struct ResourcePath{
    public string AssetsPath { get; private set;}
    public string AB_FileNmae { get; private set;}

    public ResourcePath(string ab_FileNmae, string assetsPath)
    {
        AB_FileNmae = ab_FileNmae;
        AssetsPath = assetsPath;
    }
}

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
/// 按照资源路径加载
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ResourcePathAttribute : Attribute
{
    public string AssetsPath { get; private set;}
    public string AB_FileNmae { get; private set;}

    public ResourcePathAttribute(string ab_FileNmae, string assetsPath)
    {
        AB_FileNmae = ab_FileNmae;
        AssetsPath = assetsPath;
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

