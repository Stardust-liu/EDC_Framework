using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotItem
{
    public int ActiveCount{get; private set;}
    private RedDotLeafNode redDotNode;

    public RedDotItem(RedDotLeafNode redDotLeafNode){
        redDotNode = redDotLeafNode;
    }

    public void SetActiveCount(int activeCount){
        ActiveCount = activeCount;
    }

    public int ActiveRedDot(){
        ActiveCount++;
        if(ActiveCount == 1){
            Hub.RedDotTree.ActiveRedDot(redDotNode);
        }
        return ActiveCount;
    }

    public int DisableRedDot(){
        ActiveCount--;
        if(ActiveCount == 0){
            Hub.RedDotTree.DisableRedDot(redDotNode);
        }
        return ActiveCount;
    }
}
