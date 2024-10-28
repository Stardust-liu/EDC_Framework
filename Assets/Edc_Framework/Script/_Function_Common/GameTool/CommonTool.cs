using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CommonTool
{
    /// <summary>
    /// 字符串转换为对象数据
    /// </summary>
    public static T StringToClass<T>(string value) where T : BaseStringParser{
        var obj = Activator.CreateInstance<T>();
        obj.Init(StringSplit(value, "#"));
        return obj;
    }

    /// <summary>
    /// 字符串转换为对象数据
    /// </summary>
    public static T StringToClass<T>(string[] data) where T : BaseStringParser{
        var obj = Activator.CreateInstance<T>();
        obj.Init(data);
        return obj;
    }

    /// <summary>
    /// 拆分字符串
    /// </summary>
    public static string[] StringSplit(string value, string delimiter){
        return value.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
