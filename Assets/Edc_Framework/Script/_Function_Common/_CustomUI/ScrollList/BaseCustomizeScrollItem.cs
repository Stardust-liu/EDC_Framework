using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BaseCustomizeScrollItem<T> : MonoBehaviour
{
    public int CurrentDataIndex { get; private set;}
    public Vector2 Pos { get{return ((RectTransform)transform).anchoredPosition;} }
    private T value;
    public virtual void UpdateData(int index, T value){
        CurrentDataIndex = index;
        this.value = value;
    }

    public void UpdatePos(Vector2 targetPos, Vector2 offset){
        ((RectTransform)this.transform).anchoredPosition = targetPos + offset;
    }
}
