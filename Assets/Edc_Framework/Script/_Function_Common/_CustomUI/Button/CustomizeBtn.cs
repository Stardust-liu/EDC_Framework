using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizeBtn : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private bool m_IsScrollRectChild;
    public bool IsScrollRectChild{
        get{
            return m_IsScrollRectChild;
        }
        set{
            m_IsScrollRectChild = value;
        }
    }

    [SerializeField]
    private ScrollRect m_ScrollRect;
    public ScrollRect ScrollRect{
        get{
            return m_ScrollRect;
        }
        set{
            m_ScrollRect = value;
        }
    }
    
    [SerializeField]
    private TweenGroup m_PointerEnterTween;
    public TweenGroup PointerEnterTween{
        get{
            return m_PointerEnterTween;
        }
        set{
            m_PointerEnterTween = value;
        }
    }

    [SerializeField]
    private TweenGroup m_PointerExitTween;
    public TweenGroup PointerExitTween{
        get{
            return m_PointerExitTween;
        }
        set{
            m_PointerExitTween = value;
        }
    }

    [SerializeField]
    private TweenGroup m_PointerDownTween;
    public TweenGroup PointerDownTween{
        get{
            return m_PointerDownTween;
        }
        set{
            m_PointerDownTween = value;
        }
    }

    [SerializeField]
    private TweenGroup m_PointerDownEndTween;
    public TweenGroup PointerDownEndTween{
        get{
            return m_PointerDownEndTween;
        }
        set{
            m_PointerDownEndTween = value;
        }
    }

    [SerializeField]
    private BtnClickAudioType m_ClickAudioType;

    public BtnClickAudioType ClickAudioType{
        get{
            return m_ClickAudioType;
        }
        set{
            m_ClickAudioType = value;
        }
    }


    private bool isPlayPointerExitTween;
#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        transition = Transition.None;
    }
#endif

    protected override void Start()
    {
        base.Start();
        if(m_IsScrollRectChild && m_ScrollRect != null){
            TryGetScrollRect();
        }
    }

    public void TryGetScrollRect(){
        m_ScrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(m_IsScrollRectChild){
            m_ScrollRect.OnBeginDrag(eventData);
            m_PointerEnterTween.Play();
            m_PointerDownEndTween.Play();
            isPlayPointerExitTween = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(m_IsScrollRectChild){
            m_ScrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(m_IsScrollRectChild){
            m_ScrollRect.OnEndDrag(eventData);
            isPlayPointerExitTween = false;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        m_PointerEnterTween.Play();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if(!isPlayPointerExitTween){
            m_PointerExitTween.Play();
            m_PointerDownEndTween.Play();
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if(m_ClickAudioType != BtnClickAudioType.None){
            Hub.Audio.PlaySoundEffect(BtnClickAudioInfoCfg.Instance.GetPath(m_ClickAudioType));
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        m_PointerDownTween.Play();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        m_PointerDownEndTween.Play();
    }
}
