using System.Collections;
using UnityEngine;
using XNode;
using XNodeEditor;
using System;

[CustomNodeGraphEditor(typeof(RedDotTreeGraph))]
public class RedDotTreeGraphEditor : NodeGraphEditor{
    public override Node CreateNode(Type type, Vector2 position)
    {
        Node node = base.CreateNode(type, position);
        var name = (ScriptableObject.CreateInstance(type) as BaseNode).GetNodeTitle();
        node.name = name;
        return node;
    }
}
