using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class OptimizationScroll_H<T> : BaseOptimizationScroll<T>
{
    [PropertyOrder(-1)]
    public HorizontalLayoutGroup horizontalLayoutGroup;
    private float itemOccupyW;

    protected override void SetItemInterval()
    {
        horizontalLayoutGroup.spacing = itemInterval;
        SaveLayoutGroupState();
    }

    protected override void SetStartInterval()
    {
        horizontalLayoutGroup.padding.left = headInterval;
        SaveLayoutGroupState();
    }

    protected override void SetEndInterval()
    {
        horizontalLayoutGroup.padding.right = tailInterval;
        SaveLayoutGroupState();
    }

    protected override void SaveLayoutGroupState()
    {
#if UNITY_EDITOR
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);
        EditorUtility.SetDirty(horizontalLayoutGroup);
        AssetDatabase.SaveAssets();
#endif
    }
    
    protected override void CheckHeadItem(){
        if(item[t].CurrentDataIndex == dataMaxIndex)
            return;
        if(item[h].Pos.x + ContentPos.x < headUpdatePosEdge){          
            item[h].UpdatePos(item[t].Pos, Vector2.right * (itemOccupyW + itemInterval));
            base.CheckHeadItem();
        }
    }
     
    protected override void CheckTailItem(){
        if(item[h].CurrentDataIndex == 0)
            return;
        if(item[t].Pos.x + ContentPos.x > tailUpdatePosEdge){
            item[t].UpdatePos(item[h].Pos, Vector2.left * (itemOccupyW + itemInterval));
            base.CheckTailItem();
        }
    }
    
    public override void Start(){
        base.Start();
        var itemIntervalHalf = itemInterval/2f;
        direction = Vector2.right;
        itemOccupyW = ((RectTransform)item[0].transform).rect.width;
        headUpdatePosEdge = - itemOccupyW - itemIntervalHalf;
        tailUpdatePosEdge = viewportSize.x + itemOccupyW + itemIntervalHalf;
        horizontalLayoutGroup.enabled = false;
        var contentParentWidth = dataCount * itemOccupyW + dataMaxIndex * itemInterval + headInterval + tailInterval;
        var contentParentHeight = contentParent.sizeDelta.y;
        var horizontalScrollbar = scrollRect.horizontalScrollbar;
        contentParent.sizeDelta = new Vector2(contentParentWidth, contentParentHeight);
        if(horizontalScrollbar != null){
            var eventTrigger = horizontalScrollbar.GetComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener(ApplicationInertia);
            var dragStateEvent = new List<EventTrigger.Entry>(){
                pointerDown,
            };
            eventTrigger.triggers = dragStateEvent;
        }
    }
}
