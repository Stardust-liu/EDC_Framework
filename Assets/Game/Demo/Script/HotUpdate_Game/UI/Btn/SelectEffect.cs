using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEffect : MonoBehaviour
{
    public RectTransform rectTransform;
    public Animator selectEffect;
    private Action confirmSelectComplete;

    /// <summary>
    /// 切换选项
    /// </summary>
    public void ChangeOptionItem(Transform parent)
    {
        selectEffect.SetTrigger("Show");
        rectTransform.SetParent(parent, false);
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public void Hide()
    {
        selectEffect.SetTrigger("Hide");
    }

    /// <summary>
    /// 确认选择
    /// </summary>
    public void ConfirmSelect(Action _confirmSelectComplete)
    {
        Hub.Interaction.DisableInteraction();
        confirmSelectComplete = _confirmSelectComplete;
        selectEffect.SetTrigger("ConfirmSelect");
    }

    /// <summary>
    /// 重置位置
    /// </summary>
    public void SetPosZero()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }

    protected void ConfirmSelectComplete()
    {
        confirmSelectComplete?.Invoke();
        confirmSelectComplete = null;
        Hub.Interaction.EnableInteraction();
        //↑必须在confirmSelectComplete后调用，confirmSelectComplete中可能有地方也要调用 禁止交互
    }
}
