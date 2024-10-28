using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseInfiniteScrollItem<T> : BaseCustomizeScrollItem<T>
{
    private Vector2 beginPos;
    private Vector2 beginTouchPos;
    public void BeginDrag(Vector2 beginDragPos){
        beginPos = ((RectTransform)this.transform).anchoredPosition;
        beginTouchPos = beginDragPos;
    }

    public void Drag(Vector2 dragPos){
        ((RectTransform)this.transform).anchoredPosition = beginPos + (dragPos - beginTouchPos);
    }

    public void UpdatePos(Vector2 targetPos, Vector2 offset, Vector2 dragPos){
        UpdatePos(targetPos, offset);
        beginPos = ((RectTransform)this.transform).anchoredPosition;
        beginTouchPos = dragPos;
    }

    public void InertiaOffset(Vector2 offset){
        ((RectTransform)this.transform).anchoredPosition += offset;
    }
}
