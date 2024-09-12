using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 末端节点
/// </summary>
public enum RedDotLeafNode{
    SystemInformation1 = 100_1,
    SystemInformation2 = 100_2,
    PersonalMessage1 = 200_1,
    Task1 = 300_1,
    Task2 = 300_2,
}


/// <summary>
/// 起点与分支节点
/// </summary>
public enum RedDotNode{
    Notify = 1,
    Task = 2,
    SystemInformation = 100, //系统消息
    PersonalMessage = 200, //个人消息
}
