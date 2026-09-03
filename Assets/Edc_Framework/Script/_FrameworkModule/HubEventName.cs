using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 开始加载场景
/// </summary>
public class LoadSceneBegin{
    public string sceneName;
    public LoadSceneBegin(string _sceneName){
        sceneName = _sceneName;
    }
}

/// <summary>
/// 加载场景结束
/// </summary>
public class LoadSceneEnd{
    public string sceneName;
    public LoadSceneEnd(string _sceneName){
        sceneName = _sceneName;
    }
}

/// <summary>
/// 更新UI边距
/// </summary>
public class UpdateMargins
{

}

/// <summary>
/// 准备改变语言
/// </summary>
public class ReadyChangeLanguage
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
public class ChangeLanguage
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
public class UnlockAchievement{
    public string unlockAchievement;
    public UnlockAchievement(string _unlockAchievement){
        unlockAchievement = _unlockAchievement;
    }
}

/// <summary>
/// 更新成就进度
/// </summary>
public class UpdateAchievementSchedule{
    public string unlockAchievement;
    public int schedule;
    public UpdateAchievementSchedule(string _unlockAchievement, int _schedule){
        unlockAchievement = _unlockAchievement;
        schedule = _schedule;
    }
}

/// <summary>
/// 更新红点状态（起点与分支节点）
/// </summary>
public class UpdateRedDotNodeState{
    public RedDotNode redDotNode;
    public UpdateRedDotNodeState(RedDotNode _redDotNode){
        redDotNode = _redDotNode;
    }
}

/// <summary>
/// 更新红点状态（末端节点）
/// </summary>
public class UpdateRedDotLeafNodeState
{
    public RedDotLeafNode redDotLeafNode;
    public UpdateRedDotLeafNodeState(RedDotLeafNode _redDotLeafNode)
    {
        redDotLeafNode = _redDotLeafNode;
    }
}

public class ChangeInteractionState
{
    public bool interactionState;
    public ChangeInteractionState(bool _interactionState)
    {
        interactionState = _interactionState;
    }
}