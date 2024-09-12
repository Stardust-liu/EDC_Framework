using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[NodeTint(0.4f, 0.25f, 0.4f)]
[NodeWidth(400)]
[CreateNodeMenu("RedDotTree/LeafNode")]
public class RedDotTree_LeafNode : RedDotTreeBaseNode
{
    [Input]
    public RedDotTreeBaseNode lastNode;
    public RedDotLeafNode leafNode;

    public override string GetNodeTitle()
    {
        Debug.Log(lastNode == null);
        return "叶子节点";
    }

    /// <summary>
    /// 初始化激活状态
    /// </summary>
    public void InitActiveState(){
        if(Hub.RedDotTree.CheckRedDotState(leafNode)){
            ActiveRedDot();
        }
#if UNITY_EDITOR
        else
        {
            DisableRedDot();
        } 
#endif       
    }
    
    public override void ActiveRedDot(){
        // if(!isActive){
        //     base.ActiveRedDot();
        //     if(lastNode != null){
        //         lastNode.ActiveRedDot();
        //     }
        // }
        base.ActiveRedDot();
        if(lastNode != null){
            lastNode.ActiveRedDot();
        }
    }

    public override void DisableRedDot(){
        if(isActive){
            base.DisableRedDot();
            if(lastNode != null){
                lastNode.DisableRedDot();
            }
        }
    }
}
