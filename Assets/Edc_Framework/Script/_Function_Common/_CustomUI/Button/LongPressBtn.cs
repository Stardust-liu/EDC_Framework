using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressBtn : CustomizeBtn
{   
    [SerializeField]
    private float m_EnterContinuousPressStateTime = 0.5f;
    public float EnterContinuousPressStateTime{
        get { return m_EnterContinuousPressStateTime; }
        set { m_EnterContinuousPressStateTime = value; }
    }


    [SerializeField]
    private float m_CallInterval = 0.2f;
    public float PerformSpeed{
        get { return m_CallInterval; }
        set { m_CallInterval = value; }
    }


    [Serializable]
    public class LongPressEvent : UnityEvent {}

    [SerializeField]
    private LongPressEvent m_OnLongPress = new LongPressEvent();
    public LongPressEvent OnOnLongPressDown
    {
        get { return m_OnLongPress; }
        set { m_OnLongPress = value; }
    }

    private WaitForSecondsRealtime prePressWaitTime;
    private WaitForSecondsRealtime callIntervalWaitTime;
    private IEnumerator continuous;

    protected override void Start()
    {
        base.Start();
        prePressWaitTime = new WaitForSecondsRealtime(m_EnterContinuousPressStateTime);
        callIntervalWaitTime = new WaitForSecondsRealtime(m_CallInterval);
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        continuous = Continuous();
        StartCoroutine(continuous);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        StopCoroutine(continuous);
        continuous = null;
    }

    public void UpdatePrePressTime(float value){
        m_EnterContinuousPressStateTime = value;
        prePressWaitTime = new WaitForSecondsRealtime(m_EnterContinuousPressStateTime);
    }

    public void UpdateCallIntervalTime(float value){
        m_CallInterval = value;
        callIntervalWaitTime = new WaitForSecondsRealtime(m_CallInterval);
    }

    private IEnumerator Continuous()
    {
        yield return prePressWaitTime;
        while (true)
        {
            yield return callIntervalWaitTime;
            m_OnLongPress.Invoke();
        }
    }
}
