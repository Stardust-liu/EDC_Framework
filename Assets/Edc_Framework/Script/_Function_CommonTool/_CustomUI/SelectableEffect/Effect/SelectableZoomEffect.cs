using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct ZoomChangeInfo
{
    public RectTransform target;
    public float normal;
    public float highlighted;
    public float selected;
    public float pressed;
    public float disabled;
}

public class SelectableZoomEffect : BaseSelectableEffect
{
    [Range(0,1f)]
    public float durationFactor = 1;

    [TableList] 
    public ZoomChangeInfo[] zoomChangeInfos;

    protected override void ChangeState()
    {
        UpdateScale();
    }

    private void UpdateScale()
    {
        var lerpTime = AnimTime * durationFactor;
        foreach (var item in zoomChangeInfos)
        {
            var TargetScale = GetTargetScale(item);
            var targetImage = item.target;
            targetImage.DOKill();
            targetImage.DOScale(new Vector3(TargetScale, TargetScale, 1), lerpTime);
        }
    }

    private float GetTargetScale(ZoomChangeInfo zoomChangeInfo)
    {
        switch (CurrentState)
        {
            case "Normal":
                return zoomChangeInfo.normal;
            case "Highlighted":
                return zoomChangeInfo.highlighted;
            case "Selected":
                return zoomChangeInfo.selected;
            case "Pressed":
                return zoomChangeInfo.pressed;
            default:
                return zoomChangeInfo.disabled;
        }
    }
}
