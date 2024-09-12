using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseOptimizationScroll<T> : BaseCustomizeScroll<T>
{
    [OnValueChanged("SetStartInterval")]
    public int headInterval;
    [OnValueChanged("SetEndInterval")]
    public int tailInterval;
    public ScrollRect scrollRect;
    public BaseOptimizationScrollItem<T>[] item;
    private IEnumerator repeatCheckIsNeedPadding;
    protected Vector2 ContentPos {get{return contentParent.anchoredPosition;}}
    protected abstract void SetStartInterval();
    protected abstract void SetEndInterval();

    protected void ApplicationInertia(Vector2 pos){
        CheckIsNeedPadding();
    }

    protected void ApplicationInertia(BaseEventData baseEventData){
        if(repeatCheckIsNeedPadding != null){
            StopCoroutine(repeatCheckIsNeedPadding);
        }
        repeatCheckIsNeedPadding = RepeatCheckIsNeedPadding();
        StartCoroutine(repeatCheckIsNeedPadding);
    }

    private IEnumerator RepeatCheckIsNeedPadding(){
        for (int i = 0; i < itemCount; i++)
        {
            CheckIsNeedPadding();
            yield return null;
        }
        repeatCheckIsNeedPadding = null;
    }

    public override void Start(){
        base.Start();
        var contentRect = ((RectTransform)scrollRect.transform).rect;
        itemCount = item.Length;
        t = itemCount-1;
        viewportSize = new Vector2(contentRect.width, contentRect.height);
        scrollRect.onValueChanged.AddListener(ApplicationInertia);
        InitData();
    }

    protected override void InitData(){
        base.InitData();
        if(itemCount > dataCount){
            LogManager.LogError("创建的元素超过了，指定的数据数量");
            return;
        }
        for (int i = 0; i < itemCount; i++)
        {
            item[i].UpdateData(i, scrollData[i]);
        }
    }

    /// <summary>
    /// 更新头部数据
    /// </summary>
    protected override void CheckHeadItem(){
        var i = GetCorrectIndex(item[t].CurrentDataIndex +1);
        item[h].UpdateData(i, scrollData[i]);
        base.CheckHeadItem();
    }

    /// <summary>
    /// 更新尾部数据
    /// </summary>
    protected override void CheckTailItem(){
        var i = GetCorrectIndex(item[h].CurrentDataIndex -1);
        item[t].UpdateData(i, scrollData[i]);
        base.CheckTailItem();
    }
}
