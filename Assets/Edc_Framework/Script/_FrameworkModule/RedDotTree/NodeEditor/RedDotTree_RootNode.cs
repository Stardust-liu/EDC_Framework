using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[NodeTint(0.4f, 0.25f, 0.25f)]
[CreateNodeMenu("RedDotTree/RootNode")]
public class RedDotTree_RootNode : RedDotTree_BranchNode
{
    public override string GetNodeTitle()
    {
        return "根节点";
    }

    public override void SetlastNodeInfo()
    {
        //根节点不需要设置父节点的信息
    }
}
