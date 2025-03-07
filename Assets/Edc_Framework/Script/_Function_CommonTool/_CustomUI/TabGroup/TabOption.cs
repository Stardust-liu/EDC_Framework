using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TabOptionBase: Button
{
    public static bool isHaveSelectStateTab;

    [SerializeField]
    protected bool m_IsScrollRectChild;
    public bool IsScrollRectChild{
        get{
            return m_IsScrollRectChild;
        }
        set{
            m_IsScrollRectChild = value;
        }
    }

    [SerializeField]
    protected ScrollRect m_ScrollRect;
    public ScrollRect ScrollRect{
        get{
            return m_ScrollRect;
        }
        set{
            m_ScrollRect = value;
        }
    }


    [SerializeField]
    protected TweenGroup m_PointerEnterTween;
    public TweenGroup PointerEnterTween{
        get{
            return m_PointerEnterTween;
        }
        set{
            m_PointerEnterTween = value;
        }
    }

    [SerializeField]
    protected TweenGroup m_PointerExitTween;
    public TweenGroup PointerExitTween{
        get{
            return m_PointerExitTween;
        }
        set{
            m_PointerExitTween = value;
        }
    }

    [SerializeField]
    protected TweenGroup m_SelectTween;
    public TweenGroup SelectTween{
        get{
            return m_SelectTween;
        }
        set{
            m_SelectTween = value;
        }
    }

    [SerializeField]
    protected TweenGroup m_CancelSelectTween;
    public TweenGroup CancelSelectTween{
        get{
            return m_CancelSelectTween;
        }
        set{
            m_CancelSelectTween = value;
        }
    }

    [SerializeField]
    protected TweenGroup m_PointerDownTween;
    public TweenGroup PointerDownTween{
        get{
            return m_PointerDownTween;
        }
        set{
            m_PointerDownTween = value;
        }
    }

    [SerializeField]
    protected TweenGroup m_PointerDownEndTween;
    public TweenGroup PointerDownEndTween{
        get{
            return m_PointerDownEndTween;
        }
        set{
            m_PointerDownEndTween = value;
        }
    }

    [SerializeField]
    protected bool m_IsCloseItself;
    public bool IsCloseItself{
        get{
            return m_IsCloseItself;
        }
        set{
            m_IsCloseItself = value;
        }
    }


    [SerializeField]
    protected BtnClickAudioType m_UnSelectAudioType;

    public BtnClickAudioType UnSelectAudioType{
        get{
            return m_UnSelectAudioType;
        }
        set{
            m_UnSelectAudioType = value;
        }
    }

    [SerializeField]
    protected BtnClickAudioType m_SelectAudioType;

    public BtnClickAudioType SelectAudioType{
        get{
            return m_SelectAudioType;
        }
        set{
            m_SelectAudioType = value;
        }
    }
#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        transition = Transition.None;
    }
#endif

    public abstract void OnSelect();
    public abstract void OnCancelSelect();
}

public class TabOption<T> : TabOptionBase , IDragHandler, IBeginDragHandler, IEndDragHandler
where T: TabOption<T>
{
    protected static T currentSelect;
    private bool isPlayPointerExitTween;

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

    /// <summary>
    /// 选择
    /// </summary>
    public override void OnSelect()
    {
        base.Select();
        if (currentSelect == null)
        {
            isHaveSelectStateTab = true;
            currentSelect = this as T;
            m_SelectTween.Play();
        }
        else
        {
            if (m_IsCloseItself)
            {
                if (currentSelect == this as T)
                {
                    currentSelect.OnCancelSelect();
                    currentSelect = null;
                    isHaveSelectStateTab = false;
                }
                else
                {
                    currentSelect.OnCancelSelect();
                    currentSelect = this as T;
                }
            }
            else
            {
                if (currentSelect != this)
                {
                    currentSelect.OnCancelSelect();
                    currentSelect = this as T;
                }
            }
            m_SelectTween.Play();
        }
    }

    /// <summary>
    /// 取消选择
    /// </summary>
    public override void OnCancelSelect()
    {
        m_CancelSelectTween.Play();
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

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        OnSelect();
        if(currentSelect != this){
            if(m_SelectAudioType != BtnClickAudioType.None){
                Hub.Audio.PlaySoundEffect(BtnClickAudioInfoCfg.Instance.GetPath(m_UnSelectAudioType));
            }
        }
        else{
            if(m_UnSelectAudioType != BtnClickAudioType.None){
                Hub.Audio.PlaySoundEffect(BtnClickAudioInfoCfg.Instance.GetPath(m_SelectAudioType));
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if(currentSelect != this){
            m_PointerEnterTween.Play();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if(currentSelect != this && !isPlayPointerExitTween){
            m_PointerExitTween.Play();
            m_PointerDownEndTween.Play();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if(currentSelect != this){
            m_PointerDownTween.Play();
        }
    }

    public T GetCurrentSelect(){
        return currentSelect;
    }
}
