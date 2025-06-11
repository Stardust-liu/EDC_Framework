using System;
using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using UnityEngine;

public class RedDotTreeManager : BaseIOCComponent<RedDotData>, ISendEvent
{
    private RedDotTreeSetting redDotTreeSetting;
    private static bool isSendNotification;

    protected override void Init(){
        base.Init();
        var resourcePath = new ResourcePath("RedDotTreeSetting","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/RedDotTree/RedDotTreeSetting.asset");
        redDotTreeSetting = Hub.Resources.GetScriptableobject<RedDotTreeSetting>(resourcePath);
        isSendNotification = true;
        SetInitRedDotTree();
    }

    private void SetInitRedDotTree(){
        if(!Data.isInitSave){
            var redDotLeafNodeArray = Enum.GetValues(typeof(RedDotLeafNode));
            var redDotNodeArray = Enum.GetValues(typeof(RedDotNode));
            foreach (RedDotLeafNode item in redDotLeafNodeArray)
            {
                Data.leafRedDotState.Add(item, false);
            }
            foreach (RedDotNode item in redDotNodeArray)
            {
                Data.nonLeafRedDotState.Add(item, false);
            }
        }
        //data.SaveDataNow();
    }
    
    /// <summary>
    /// 更新起点或分支节点状态
    /// </summary>
    public void UpdateRedDotState(RedDotNode redDotNode, bool isActive){
        Data.UpdateRedDotState(redDotNode, isActive);
        if(isSendNotification){
            this.SendEvent(new UpdateRedDotNodeState(redDotNode));
        }
    }

    /// <summary>
    /// 激活红点
    /// </summary>
    public void ActiveRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.ActiveRedDot(redDotLeafNode);
        Data.ActiveRedDot(redDotLeafNode);
        if(isSendNotification){
            this.SendEvent(new UpdateRedDotLeafNodeState(redDotLeafNode));
        }
    }

    /// <summary>
    /// 隐藏红点
    /// </summary>
    public void DisableRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.DisableRedDot(redDotLeafNode);
        Data.DisableRedDot(redDotLeafNode);
        if(isSendNotification){
            this.SendEvent(new UpdateRedDotLeafNodeState(redDotLeafNode));
        }
    }

    /// <summary>
    /// 检查起点与分支红点状态
    /// </summary>
    public bool CheckRedDotState(RedDotNode redDotNode){
        return Data.nonLeafRedDotState[redDotNode];
    }

    /// <summary>
    /// 检查末端红点状态
    /// </summary>
    public bool CheckRedDotState(RedDotLeafNode redDotLeafNode){
        return Data.leafRedDotState[redDotLeafNode];
    }
}
