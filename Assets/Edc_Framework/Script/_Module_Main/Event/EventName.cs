using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventName
{
    public const string testEventName = "testEventName";
    public const string loadSceneBegin = "loadSceneBegin";
    public const string loadSceneEnd = "loadSceneEnd";

    /// <summary>
    /// 进入禁止交互模式
    /// </summary>
    public const string enterRestriction = "enterRestriction";

    /// <summary>
    /// 退出禁止交互模式
    /// </summary>
    public const string exitRestriction = "exitRestriction";

    /// <summary>
    /// 修改语言
    /// </summary>
    public const string changeLanguage = "changeLanguage";
}
