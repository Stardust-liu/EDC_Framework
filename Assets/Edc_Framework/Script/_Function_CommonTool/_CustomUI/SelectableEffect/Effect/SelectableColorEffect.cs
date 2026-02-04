using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ColorChangeInfo
{
    public Image target;
    public Color normal;
    public Color highlighted;
    public Color selected;
    public Color pressed;
    public Color disabled;
}

public class SelectableColorEffect : BaseSelectableEffect
{
    [Range(0,1f)]
    public float durationFactor = 1;
    
    [TableList] 
    public ColorChangeInfo[] colorChangeInfos;

    protected override void SetState()
    {
        foreach (var item in colorChangeInfos)
        {
            var targetColor = GetTargetColor(item);
            item.target.color = targetColor;
        }
    }

    protected override void ChangeState()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        var lerpTime = AnimTime * durationFactor;
        foreach (var item in colorChangeInfos)
        {
            var targetColor = GetTargetColor(item);
            var targetImage = item.target;
            targetImage.DOKill();
            targetImage.DOColor(targetColor, lerpTime);
        }
    }

    private Color GetTargetColor(ColorChangeInfo colorChangeInfo)
    {
        switch (CurrentState)
        {
            case "Normal":
                return colorChangeInfo.normal;
            case "Highlighted":
                return colorChangeInfo.highlighted;
            case "Selected":
                return colorChangeInfo.selected;
            case "Pressed":
                return colorChangeInfo.pressed;
            default:
                return colorChangeInfo.disabled;
        }
    }
}
