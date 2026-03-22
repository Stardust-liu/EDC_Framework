using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Sirenix.OdinInspector;
using UnityEditor;
public abstract class RedDotTreeBaseNode : BaseNode 
{
    [Input, ShowIf(nameof(IshavelastNode))]
    public RedDotTreeBaseNode lastNode;
    [ReadOnly]
    public bool isActive;
    [HideInInspector]
    public bool lastIsActiveState;
    public virtual bool IshavelastNode{ get {return false;}}

    /// <summary>
    /// 激活红点
    /// </summary>
    public virtual void ActiveRedDot(){
        isActive = true;
    }

    /// <summary>
    /// 隐藏红点
    /// </summary>
    public virtual void DisableRedDot(){
        isActive = false;
    }

    /// <summary>
    /// 设置上一个节点的信息
    /// </summary>
    public virtual void SetlastNodeInfo()
    {
        if (isActive)
        {
            lastNode.ActiveRedDot();
        }
        else
        {
            lastNode.DisableRedDot();
        }
        Hub.RedDotTree.AddNextList(lastNode);
    }

    public virtual void SetInfo()
    {
        lastIsActiveState = isActive;
    }

    /// <summary>
    /// 应用最终结果
    /// </summary>
    public abstract void ApplyEndResult();
}
