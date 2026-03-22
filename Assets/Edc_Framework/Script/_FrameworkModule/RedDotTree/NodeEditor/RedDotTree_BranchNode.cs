using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

public abstract class RedDotTree_BranchNode : RedDotTreeBaseNode
{
    [ReadOnly]
    public int activeCount;
    [Output]
    public RedDotTreeBaseNode[] childNode;
    public RedDotNode node;

    public override void SetInfo()
    {
        var redDotTree = Hub.RedDotTree;
        isActive = redDotTree.GetRedDotState(node);
        activeCount = redDotTree.GetRedDotActiveCount(node);
        base.SetInfo();
    }

    public override void ActiveRedDot(){
        activeCount++;
        if(activeCount == 1){
            base.ActiveRedDot();
        }
    }

    public override void DisableRedDot(){
        activeCount--;
        if(activeCount == 0){
            base.DisableRedDot();
        }
    }

    /// <summary>
    /// 应用最终结果
    /// </summary>
    public override void ApplyEndResult()
    {
        if (lastIsActiveState != isActive)
        {
            var redDotTree = Hub.RedDotTree;
            lastIsActiveState = isActive;
            redDotTree.AddCurrentList(this);
            redDotTree.UpdateRedDotState(node, isActive, activeCount);
        }
    }

    public override object GetValue(NodePort port) {
        return null; //防止鼠标悬停在Input附近时报警告
    }
}
