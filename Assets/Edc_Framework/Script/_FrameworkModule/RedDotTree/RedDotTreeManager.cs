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
    private static bool isSendNotification;
    private RedDotData data;
    public static readonly EventCenter eventCenter = new EventCenter();

    public RedDotTreeManager(){
        data = GameArchive.RedDotData;
        redDotTreeSetting = Hub.Resources.GetScriptableobject<RedDotTreeSetting>(nameof(RedDotTreeSetting));
        isSendNotification = true;
        SetInitRedDotTree();
    }

    private void SetInitRedDotTree(){
        if(!data.isInitSave){
            var redDotLeafNodeArray = Enum.GetValues(typeof(RedDotLeafNode));
            var redDotNodeArray = Enum.GetValues(typeof(RedDotNode));
            foreach (RedDotLeafNode item in redDotLeafNodeArray)
            {
                data.leafRedDotState.Add(item, false);
            }
            foreach (RedDotNode item in redDotNodeArray)
            {
                data.nonLeafRedDotState.Add(item, false);
            }
        }
        data.SaveDataNow();
    }
    
    /// <summary>
    /// 更新起点或分支节点状态
    /// </summary>
    public void UpdateRedDotState(RedDotNode redDotNode, bool isActive){
        data.UpdateRedDotState(redDotNode, isActive);
        if(isSendNotification){
            eventCenter.OnEvent(redDotNode.ToString());
        }
    }

    /// <summary>
    /// 激活红点
    /// </summary>
    public void ActiveRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.ActiveRedDot(redDotLeafNode);
        data.ActiveRedDot(redDotLeafNode);
        if(isSendNotification){
            eventCenter.OnEvent(redDotLeafNode.ToString());
        }
    }

    /// <summary>
    /// 隐藏红点
    /// </summary>
    public void DisableRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.DisableRedDot(redDotLeafNode);
        data.DisableRedDot(redDotLeafNode);
        if(isSendNotification){
            eventCenter.OnEvent(redDotLeafNode.ToString());
        }
    }

    /// <summary>
    /// 检查起点与分支红点状态
    /// </summary>
    public bool CheckRedDotState(RedDotNode redDotNode){
        return data.nonLeafRedDotState[redDotNode];
    }

    /// <summary>
    /// 检查末端红点状态
    /// </summary>
    public bool CheckRedDotState(RedDotLeafNode redDotLeafNode){
        return data.leafRedDotState[redDotLeafNode];
    }
}
