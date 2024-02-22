using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class Click : UnityEvent { }
[Serializable]
public class Down : UnityEvent { }
[Serializable]
public class Up : UnityEvent { }

public class BaseBtn : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Click onClickEvent;
    public Down onDownEvent;
    public Up onUpEvent;

    private void Start() {
        ButtonInit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp();
    }
    protected virtual void ButtonInit() { }
    protected virtual void OnDown() { if(onDownEvent != null) onDownEvent.Invoke(); }
    protected virtual void OnUp() { if(onUpEvent != null) onUpEvent.Invoke(); }
    protected virtual void OnClick() {if(onClickEvent != null) onClickEvent.Invoke(); }
}
