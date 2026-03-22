using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[NodeTint(0.25f, 0.25f, 0.4f)]
[CreateNodeMenu("RedDotTree/ChildNode")]
public class RedDotTree_ChildNode : RedDotTree_BranchNode
{
    public override bool IshavelastNode{ get {return true;}}

    public override string GetNodeTitle()
    {
        return "子节点";
    }
}
