using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;
#if UNITY_EDITOR
using XNodeEditor;
#endif

[CreateAssetMenu(fileName = "RedDotTreeSetting", menuName = "创建.Assets文件/FrameworkTool/RedDotTreeSetting")]
public class RedDotTreeSetting : SerializedScriptableObject
{
    public RedDotTreeGraph[] redDotTreeSetting;

    [ReadOnly]
    public Dictionary<RedDotLeafNode, RedDotTree_LeafNode> leafNode;

    [Button("刷新叶子节点数据", ButtonSizes.Medium), GUIColor(0.5f, 0.8f, 1f)]
    [PropertyOrder(0)]
    private void UpdateLeafNodeInfo(){
        if(leafNode != null){
            leafNode.Clear();
        }
        else{
            leafNode = new Dictionary<RedDotLeafNode, RedDotTree_LeafNode>();
        }
        var count = redDotTreeSetting.Length;
        RedDotTree_LeafNode redDotTree_LeafNode;
        List<Node> nodes;
        for (int i = 0; i < count; i++)
        {
            nodes = redDotTreeSetting[i].nodes;
            var nodeCount = nodes.Count;
            for (int j = 0; j < nodeCount; j++)
            {
                redDotTree_LeafNode = nodes[j] as RedDotTree_LeafNode;
                if(redDotTree_LeafNode != null){
                    var leafNode = redDotTree_LeafNode.leafNode;
                    if(!this.leafNode.ContainsKey(leafNode)){
                        this.leafNode.Add(leafNode, redDotTree_LeafNode);
                    }
                    else{
                        Debug.LogError($"叶子节点节点名：{leafNode} 发生重复，Graph名字为：{redDotTreeSetting[i].name}，节点索引为{j}");
                    }
                }
            }
        }
    }

    [Title("规范性检查")]
    [Button("检查可视化节点红点标签规范性", ButtonSizes.Medium), GUIColor(0.5f, 0.8f, 1)]
    private void CheckNodeSpecification(){
        var graphCount = redDotTreeSetting.Length;
        RedDotTreeGraph redDotTreeGraph;
        Node node;
        string redDotNodeString;
        for (int i = 0; i < graphCount; i++)
        {
            redDotTreeGraph = redDotTreeSetting[i];
            var nodeCount = redDotTreeGraph.nodes.Count;
            bool isSpecification;
            for (int j = 0; j < nodeCount; j++)
            {
                node = redDotTreeGraph.nodes[j];
                if(node as RedDotTree_RootNode != null){
                    redDotNodeString = ((RedDotTree_RootNode)node).node.ToString();
                    isSpecification = IsNonLeafNodeValue(redDotNodeString);
                }
                else if(node as RedDotTree_ChildNode != null){
                    redDotNodeString = ((RedDotTree_ChildNode)node).node.ToString();
                    isSpecification = IsNonLeafNodeValue(redDotNodeString);
                }
                else {
                    redDotNodeString = ((RedDotTree_LeafNode)node).leafNode.ToString();
                    isSpecification = IsLeafNodeValue(redDotNodeString);
                }

                if(!isSpecification){
                    Debug.LogError($"节点图表：{redDotTreeGraph.name}，索引编号为：[{j}] 的节点标签名（{redDotNodeString}） 不符合规范，需要修改");
                }
            }
        }
    }
    
    [ShowInInspector]
    [SerializeField]
    [PropertyOrder(1)]
    private List<GameObject> gameObjectList;
    [PropertyOrder(1)]
    [HorizontalGroup("split/left")]
    [Button("检查游戏对象红点标签规范性", ButtonSizes.Medium), GUIColor(0.5f, 0.8f, 1)]
    private void CheckGameObjectSpecification(){
        var count = gameObjectList.Count;
        if(count == 0){
            Debug.Log("需要先添加游戏对象");
            return;
        }
        for (int i = 0; i < count; i++)
        {
            var leafNodeArray = gameObjectList[i].GetComponentsInChildren<RedDotLeafNodeBtn>();
            var leafNodeArrayCount = leafNodeArray.Length;
            var nonLeafNodeArray = gameObjectList[i].GetComponentsInChildren<RedDotNonLeafNodeBtn>();
            var nonLeafNodeArrayCount = nonLeafNodeArray.Length;
            int j;
            for (j = 0; j < leafNodeArrayCount; j++)
            {
                if(!IsLeafNodeValue(leafNodeArray[j].redDotNode.ToString())){
                    Debug.LogError($"{gameObjectList[i].name}的子物体：{leafNodeArray[j].name} 的节点标签名（{leafNodeArray[j].redDotNode}） 不符合规范，需要修改");                
                }
            }
            for (j = 0; j < nonLeafNodeArrayCount; j++)
            {
                if(!IsNonLeafNodeValue(nonLeafNodeArray[j].redDotNode.ToString())){
                    Debug.LogError($"{gameObjectList[i].name}的子物体：{nonLeafNodeArray[j].name} 的节点标签名（{nonLeafNodeArray[j].redDotNode}） 不符合规范，需要修改");                
                }
            }
        }
    }

    [PropertyOrder(1)]
    [HorizontalGroup("split", 0.5f)]
    [Button("清空列表", ButtonSizes.Medium), GUIColor(1, 0.5f, 0.5f)]
    private void CleanGameObjectLiset(){
        gameObjectList.Clear();
    }

    /// <summary>
    /// 检查非叶子节点规范性
    /// </summary>
    private bool IsNonLeafNodeValue(string target){
        return Enum.IsDefined(typeof(RedDotNode), target);
    }

    /// <summary>
    /// 检查叶子节点规范性
    /// </summary>
    private bool IsLeafNodeValue(string target){
        return Enum.IsDefined(typeof(RedDotLeafNode), target);
    }


    public void InitActiveState(){
        foreach (var item in leafNode.Values)
        {
            item.InitActiveState();
        } 
    }

    public void ActiveRedDot(RedDotLeafNode redDotLeafNode){
        if(leafNode.ContainsKey(redDotLeafNode)){
            leafNode[redDotLeafNode].ActiveRedDot();
        }
        else{
            LogManager.LogError($"没有对红点标签：{redDotLeafNode} 进行设置");
        }
    }

    public void DisableRedDot(RedDotLeafNode redDotLeafNode){
        if(leafNode.ContainsKey(redDotLeafNode)){
            leafNode[redDotLeafNode].DisableRedDot();
        }
        else{
            LogManager.LogError($"没有对红点标签：{redDotLeafNode} 进行设置");
        }
    }

#if UNITY_EDITOR
    private void OpenRedDotTreeSetting(RedDotTreeGraph element)
    {
        NodeEditorWindow.Open(element);
    }
#endif
}
