using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 开始加载场景
/// </summary>
public struct LoadSceneBegin{
    public string sceneName;
    public LoadSceneBegin(string _sceneName){
        sceneName = _sceneName;
    }
}

/// <summary>
/// 加载场景结束
/// </summary>
public struct LoadSceneEnd{
    public string sceneName;
    public LoadSceneEnd(string _sceneName){
        sceneName = _sceneName;
    }
}

/// <summary>
/// 更新UI边距
/// </summary>
public struct UpdateMargins
{

}

/// <summary>
/// 准备改变语言
/// </summary>
public struct ReadyChangeLanguage
{
    public SystemLanguage currentLanguage;
    public SystemLanguage tagetLanguageId;
    public ReadyChangeLanguage(SystemLanguage _currentLanguage, SystemLanguage _tagetLanguageId)
    {
        currentLanguage = _currentLanguage;
        tagetLanguageId = _tagetLanguageId;
    }
}

/// <summary>
/// 修改语言
/// </summary>
public struct ChangeLanguage
{
    public SystemLanguage languageId;
    public ChangeLanguage(SystemLanguage _changeLanguage)
    {
        languageId = _changeLanguage;
    }
}

/// <summary>
/// 获得成就
/// </summary>
public struct UnlockAchievement{
    public string unlockAchievement;
    public UnlockAchievement(string _unlockAchievement){
        unlockAchievement = _unlockAchievement;
    }
}

/// <summary>
/// 更新红点状态（起点与分支节点）
/// </summary>
public struct UpdateRedDotNodeState{
    public RedDotNode redDotNode;
    public UpdateRedDotNodeState(RedDotNode _redDotNode){
        redDotNode = _redDotNode;
    }
}

/// <summary>
/// 更新红点状态（末端节点）
/// </summary>
public struct UpdateRedDotLeafNodeState
{
    public RedDotLeafNode redDotLeafNode;
    public UpdateRedDotLeafNodeState(RedDotLeafNode _redDotLeafNode)
    {
        redDotLeafNode = _redDotLeafNode;
    }
}

public struct ChangeInteractionState
{
    public bool interactionState;
    public ChangeInteractionState(bool _interactionState)
    {
        interactionState = _interactionState;
    }
}