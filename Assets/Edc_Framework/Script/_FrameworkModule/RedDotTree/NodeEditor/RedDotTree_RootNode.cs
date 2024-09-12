using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[NodeTint(0.4f, 0.25f, 0.25f)]
[CreateNodeMenu("RedDotTree/RootNode")]
public class RedDotTree_RootNode : RedDotTreeBaseNode
{
    [Output]
    public RedDotTreeBaseNode[] childNodes;
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
        return "根节点";
    }
    public override void ActiveRedDot(){
        activeCount++;
        if(activeCount == 1){
            base.ActiveRedDot();
            Hub.RedDotTree.UpdateRedDotState(node, true);
        }
    }

    public override void DisableRedDot(){
        activeCount--;
        if(activeCount == 0){
            base.DisableRedDot();
            Hub.RedDotTree.UpdateRedDotState(node, false);
        }
    }
}
