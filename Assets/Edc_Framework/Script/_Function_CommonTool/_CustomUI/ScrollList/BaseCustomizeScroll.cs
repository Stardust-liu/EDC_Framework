using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseCustomizeScroll<T> : MonoBehaviour
{
    [OnValueChanged("SetItemInterval")]
    public int itemInterval;
    [PropertyOrder(-2)]
    public RectTransform contentParent;
    protected int dataCount;
    protected int dataMaxIndex;
    protected int h;
    protected int t;
    protected float headUpdatePosEdge;//需要更新位置的边缘
    protected float tailUpdatePosEdge;
    protected int itemCount;
    protected Vector2 direction;
    protected Vector2 viewportSize;
    protected T[] scrollData;

    protected abstract void UpdateScrollData();

    protected abstract void SaveLayoutGroupState();

    /// <summary>
    /// 设置间隔
    /// </summary>
    protected abstract void SetItemInterval();
    
    public virtual void Start()
    {
        h = 0;
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);
        UpdateScrollData();
    }
    
    protected virtual void InitData(){
        var count = scrollData.Length;
        dataCount = count;
        dataMaxIndex = count - 1;
    }

    protected void CheckIsNeedPadding(){
        CheckHeadItem();
        CheckTailItem();
    }
    protected virtual void CheckHeadItem(){
        h = h+1 == itemCount? 0 : h+1;
        t = t+1 == itemCount? 0 : t+1;
    }

    protected virtual void CheckTailItem(){
        t = t-1 < 0? itemCount-1 : t-1;
        h = h-1 < 0? itemCount-1 : h-1;
    }

    protected int GetCorrectIndex(int index){
        if(index == -1){
            return dataMaxIndex;
        }
        else if(index == dataCount){
            return 0;
        }
        return index;
    }
}
