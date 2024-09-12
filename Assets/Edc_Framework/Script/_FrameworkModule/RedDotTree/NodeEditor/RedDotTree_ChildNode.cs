using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[NodeTint(0.25f, 0.25f, 0.4f)]
[CreateNodeMenu("RedDotTree/ChildNode")]
public class RedDotTree_ChildNode : RedDotTreeBaseNode
{
    [Input]
    public RedDotTreeBaseNode lastNode;
    [Output]
    public RedDotTreeBaseNode[] nextNode;
    [ReadOnly]
    public int activeCount;
    public RedDotNode node;

    protected override void Init()
    {
        base.Init();
        activeCount = 0;
    }

    public override string GetNodeTitle()
    {
        return "子节点";
    }

    public override void ActiveRedDot(){
        activeCount++;
        base.ActiveRedDot();
        Hub.RedDotTree.UpdateRedDotState(node, true);
        if(lastNode != null){
            lastNode.ActiveRedDot();
        }
    }

    public override void DisableRedDot(){
        activeCount--;
        if(activeCount == 0){
            base.DisableRedDot();
            Hub.RedDotTree.UpdateRedDotState(node, false);
        }
        if(lastNode != null){
            lastNode.DisableRedDot();
        }
    }
}
