using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum LogLevel
{
    Nothing,
    Everthing = DeviceInfo|Debug|Warning|Error,
    DeviceInfo = 1 << 1, //设备信息
    Debug = 1 << 2,      //调试信息
    Warning = 1 << 3,    //警告信息
    Error = 1 << 4,      //错误信息
}

public class LogManager 
{
    /// <summary>
    /// 打印设备信息
    /// </summary>
    public static void LogDeviceInfo(string message){
        if(FrameworkManager.LogDisplay.HasFlag(LogLevel.DeviceInfo)){
            Debug.Log(message);
        }
    }

    /// <summary>
    /// 打印调试信息
    /// </summary>
    public static void Log(string message){
        if(FrameworkManager.LogDisplay.HasFlag(LogLevel.Debug)){
            Debug.Log(message);
        }
    }

    /// <summary>
    /// 打印警告信息
    /// </summary>
    public static void LogWarning(string message){
        if(FrameworkManager.LogDisplay.HasFlag(LogLevel.Warning)){
            Debug.LogWarning(message);
        }
    }

    /// <summary>
    /// 打印错误信息
    /// </summary>
    public static void LogError(string message){
        if(FrameworkManager.LogDisplay.HasFlag(LogLevel.Error)){
            Debug.LogError(message);
        }
    }
}
