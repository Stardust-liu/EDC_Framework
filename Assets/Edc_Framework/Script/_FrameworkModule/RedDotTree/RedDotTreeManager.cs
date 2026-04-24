using System;
using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RedDotTreeManager : BaseIOCComponent<RedDotData>, ISendEvent
{
    private RedDotTreeSetting redDotTreeSetting;
    private HashSet<RedDotTree_LeafNode> triggerChangeLeafNode = new();
    private HashSet<RedDotTreeBaseNode> currentList = new();
    private HashSet<RedDotTreeBaseNode> nextList = new();
    private bool isWaitingForUpdate = false;

    protected override void Init(){
        base.Init();
        redDotTreeSetting = Hub.Resources.Get<RedDotTreeSetting>("RedDotTreeSetting");
        SetInitRedDotTree();
    }

    private void SetInitRedDotTree(){
        if(!Data.isInitSave){
            var redDotLeafNodeArray = Enum.GetValues(typeof(RedDotLeafNode));
            var redDotNodeArray = Enum.GetValues(typeof(RedDotNode));
            foreach (RedDotLeafNode item in redDotLeafNodeArray)
            {
                Data.UpdateleafRedDotState(item, false);
            }
            foreach (RedDotNode item in redDotNodeArray)
            {
                Data.UpdateRedDotState(item, false, 0);
            }
        }
        redDotTreeSetting.InitRedDotInfo();
        //data.SaveDataNow();
    }
    
    /// <summary>
    /// 更新根节点或分支节点状态
    /// </summary>
    public void UpdateRedDotState(RedDotNode redDotNode, bool isActive, int activeCount){
        Data.UpdateRedDotState(redDotNode, isActive, activeCount);
        this.SendEvent(new UpdateRedDotNodeState(redDotNode));
    }

    /// <summary>
    /// 更新叶子节点状态
    /// </summary>
    public void UpdateleafRedDotState(RedDotLeafNode redDotLeafNode, bool isActive)
    {
        Data.UpdateleafRedDotState(redDotLeafNode, isActive);
        this.SendEvent(new UpdateRedDotLeafNodeState(redDotLeafNode));
    }

    /// <summary>
    /// 激活红点
    /// </summary>
    public void ActiveRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.ActiveRedDot(redDotLeafNode);
    }

    /// <summary>
    /// 隐藏红点
    /// </summary>
    public void DisableRedDot(RedDotLeafNode redDotLeafNode){
        redDotTreeSetting.DisableRedDot(redDotLeafNode);
    }

    /// <summary>
    /// 获取起点与分支红点激活子节点数量
    /// </summary>
    public int GetRedDotActiveCount(RedDotNode redDotNode){
        return Data.nonLeafRedDotState[redDotNode].activeCount;
    }

    /// <summary>
    /// 获取起点与分支红点状态
    /// </summary>
    public bool GetRedDotState(RedDotNode redDotNode){
        return Data.nonLeafRedDotState[redDotNode].isActive;
    }

    /// <summary>
    /// 获取末端红点状态
    /// </summary>
    public bool GetRedDotState(RedDotLeafNode redDotLeafNode){
        return Data.leafRedDotState[redDotLeafNode];
    }

    /// <summary>
    /// 添加触发了修改操作的叶子节点
    /// </summary>
    public void AddTriggerChangeLeafNode(RedDotTree_LeafNode leafNode)
    {
        triggerChangeLeafNode.Add(leafNode);
        if (!isWaitingForUpdate)
        {
            isWaitingForUpdate = true;
            TriggerLateUpdate().Forget(); 
        }
    }

    private async UniTaskVoid TriggerLateUpdate()
    {
        await UniTask.Yield(PlayerLoopTiming.LastPreLateUpdate);        
        UpdateTreeState();
        isWaitingForUpdate = false;
    }

    /// <summary>
    /// 添加发生了修改的节点
    /// </summary>
    public void AddCurrentList(RedDotTreeBaseNode baseNode)
    {
        currentList.Add(baseNode);
    }

    /// <summary>
    /// 添加下次需要修改的节点
    /// </summary>
    public void AddNextList(RedDotTreeBaseNode branchNode)
    {
        nextList.Add(branchNode);
    }

    /// <summary>
    /// 更新整个红点数状态
    /// </summary>
    private void UpdateTreeState()
    {
        foreach (var item in triggerChangeLeafNode)
        {
            item.ApplyEndResult();
        }
        while (currentList.Count != 0)
        {
            foreach (var item in currentList)
            {
                item.SetlastNodeInfo();
            }
            currentList.Clear();
            foreach (var item in nextList)
            {
                item.ApplyEndResult();
            }
            nextList.Clear();
        }
    }
}
