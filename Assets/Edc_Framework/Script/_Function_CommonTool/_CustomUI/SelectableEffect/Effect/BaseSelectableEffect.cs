using System;
using System.Collections;
using UnityEngine;


public interface IBaseSelectableEffect
{
    void SetState(string btnState);
    void ChangeState(float animTime, string btnState);
}

public class BaseSelectableEffect : MonoBehaviour, IBaseSelectableEffect
{
    private string currentState;
    private float animTime;

    protected string CurrentState
    {
        get
        {
            return currentState; 
        }
    }

    protected float AnimTime
    {
        get
        {
            return animTime;
        }
    }

    void IBaseSelectableEffect.SetState(string _btnState)
    {
        currentState = _btnState;
        SetState();
    }

    void IBaseSelectableEffect.ChangeState(float _animTime, string _btnState)
    {
        animTime = _animTime;
        currentState = _btnState;
        ChangeState();
    }

    protected virtual void SetState(){}

    protected virtual void ChangeState(){}
}
