using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[NodeTint(0.4f, 0.25f, 0.4f)]
[NodeWidth(400)]
[CreateNodeMenu("RedDotTree/LeafNode")]
public class RedDotTree_LeafNode : RedDotTreeBaseNode
{
    public RedDotLeafNode leafNode;
    [HideInInspector]
    public bool isSetDirty;
    public override bool IshavelastNode{ get {return true;}}

    public override string GetNodeTitle()
    {
        Debug.Log(lastNode == null);
        return "叶子节点";
    }
    
    public override void SetInfo()
    {
        isActive = Hub.RedDotTree.GetRedDotState(leafNode);
        base.SetInfo();
    }
    
    public override void ActiveRedDot(){
        if(!isActive){
            base.ActiveRedDot();
            if (!isSetDirty)
            {
                isSetDirty = true;
                Hub.RedDotTree.AddTriggerChangeLeafNode(this);
            }
        }
    }

    public override void DisableRedDot(){
        if(isActive){
            base.DisableRedDot();
            if (!isSetDirty)
            {
                isSetDirty = true;
                Hub.RedDotTree.AddTriggerChangeLeafNode(this);
            }
        }
    }

    /// <summary>
    /// 应用最终结果
    /// </summary>
    public override void ApplyEndResult()
    {
        isSetDirty = false;
        if (lastIsActiveState != isActive)
        {
            lastIsActiveState = isActive;
            Hub.RedDotTree.AddCurrentList(this);
            Hub.RedDotTree.UpdateleafRedDotState(leafNode, isActive);
        }
    }
}
