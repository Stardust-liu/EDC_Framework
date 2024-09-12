using System;
using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RedDotTreeManager
{
    private RedDotTreeSetting redDotTreeSetting;
    private Dictionary<RedDotLeafNode, bool> leafRedDotState;
    private Dictionary<RedDotNode, bool> nonLeafRedDotState;
    private static bool isSendNotification;

    public static readonly EventCenter eventCenter = new EventCenter();
    private readonly RedDotData redDotData;

    public RedDotTreeManager(){
        redDotData = GameArchive.RedDotData;
        redDotTreeSetting = Hub.Framework.redDotTreeSetting;
        isSendNotification = true;
        SetInitRedDotTree();
    }

    private void SetInitRedDotTree(){
        leafRedDotState = redDotData.leafRedDotState;
        nonLeafRedDotState = redDotData.nonLeafRedDotState; 
        if(!redDotData.isInitSave){
            var redDotLeafNodeArray = Enum.GetValues(typeof(RedDotLeafNode));
            var redDotNodeArray = Enum.GetValues(typeof(RedDotNode));
            var redDotLeafNodeArrayCount = redDotLeafNodeArray.Length;
            var redDotNodeArrayCount = redDotNodeArray.Length;

            leafRedDotState = new Dictionary<RedDotLeafNode, bool>(redDotLeafNodeArrayCount);
            nonLeafRedDotState = new Dictionary<RedDotNode, bool>(redDotNodeArrayCount);

            foreach (RedDotLeafNode item in redDotLeafNodeArray)
            {
                leafRedDotState.Add(item, false);
            }
            foreach (RedDotNode item in redDotNodeArray)
            {
                nonLeafRedDotState.Add(item, false);
            }
        }
    }
    
    /// <summary>
    /// 更新起点或分支节点状态
    /// </summary>
    public void UpdateRedDotState(RedDotNode redDotNode, bool isActive){
        nonLeafRedDotState[redDotNode] = isActive;
        redDotData.UpdateRedDotState(redDotNode, isActive);
        if(isSendNotification){
            eventCenter.OnEvent(redDotNode.ToString());
        }
    }

    /// <summary>
    /// 激活红点
    /// </summary>
    public void ActiveRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.ActiveRedDot(redDotLeafNode);
        redDotData.ActiveRedDot(redDotLeafNode);
        if(isSendNotification){
            eventCenter.OnEvent(redDotLeafNode.ToString());
        }
    }

    /// <summary>
    /// 隐藏红点
    /// </summary>
    public void DisableRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.DisableRedDot(redDotLeafNode);
        redDotData.DisableRedDot(redDotLeafNode);
        if(isSendNotification){
            eventCenter.OnEvent(redDotLeafNode.ToString());
        }
    }

    /// <summary>
    /// 检查起点与分支红点状态
    /// </summary>
    public bool CheckRedDotState(RedDotNode redDotNode){
        return nonLeafRedDotState[redDotNode];
    }

    /// <summary>
    /// 检查末端红点状态
    /// </summary>
    public bool CheckRedDotState(RedDotLeafNode redDotLeafNode){
        return leafRedDotState[redDotLeafNode];
    }
}
