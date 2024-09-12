using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class OptimizationScroll_V<T> : BaseOptimizationScroll<T>
{
    [PropertyOrder(-1)]
    public VerticalLayoutGroup verticalLayoutGroup;
    private float itemOccupyH;

    protected override void SetItemInterval()
    {
        verticalLayoutGroup.spacing = itemInterval;
        SaveLayoutGroupState();
    }

    protected override void SetStartInterval()
    {
        verticalLayoutGroup.padding.top = headInterval;
        SaveLayoutGroupState();
    }

    protected override void SetEndInterval()
    {
        verticalLayoutGroup.padding.bottom = tailInterval;
        SaveLayoutGroupState();
    }

    protected override void SaveLayoutGroupState()
    {
#if UNITY_EDITOR
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);
        EditorUtility.SetDirty(verticalLayoutGroup);
        AssetDatabase.SaveAssets();
#endif
    }

    protected override void CheckHeadItem(){
        if(item[t].CurrentDataIndex == dataMaxIndex)
            return;
        if(item[h].Pos.y + ContentPos.y > headUpdatePosEdge){
            item[h].UpdatePos(item[t].Pos, Vector2.down * (itemOccupyH + itemInterval));
            base.CheckHeadItem();
        }
    }

    protected override void CheckTailItem(){
        if(item[h].CurrentDataIndex == 0)
            return;
        if(item[t].Pos.y + ContentPos.y < tailUpdatePosEdge){
            item[t].UpdatePos(item[h].Pos, Vector2.up * (itemOccupyH + itemInterval));
            base.CheckTailItem();
        }
    }

    public override void Start(){
        base.Start();
        var itemIntervalHalf = itemInterval/2f;
        direction = Vector2.up;
        itemOccupyH = ((RectTransform)item[0].transform).rect.height;
        headUpdatePosEdge = itemOccupyH + itemIntervalHalf;
        tailUpdatePosEdge = - viewportSize.y - itemOccupyH - itemIntervalHalf;
        verticalLayoutGroup.enabled = false;
        var contentParentWidth = contentParent.sizeDelta.x;
        var contentParentHeight = dataCount * itemOccupyH + dataMaxIndex * itemInterval + headInterval + tailInterval;
        var verticalScrollbar = scrollRect.verticalScrollbar;
        contentParent.sizeDelta = new Vector2(contentParentWidth, contentParentHeight);
        if(verticalScrollbar != null){
            var eventTrigger = verticalScrollbar.GetComponent<EventTrigger>();
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
