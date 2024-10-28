using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenSize : BaseTween
{
    public Vector2 startSize;
    public Vector2 targetSize;
    public RectTransform rectTransform;

    private void Reset()
    {
        TryGetComponent(out RectTransform _rectTransform);
        if(_rectTransform != null){
            rectTransform = _rectTransform;
            startSize = targetSize = rectTransform.sizeDelta;
        }
    }

     private void Start() {
        SetToStart();
    }

    protected override Tweener ForwardPlay(){
        return Play(targetSize);
    }
    protected override Tweener ReversePlay(){
        return Play(startSize);
    }

    private Tweener Play(Vector3 targetSize){
        Tweener tweener;
        tweener = rectTransform.DOSizeDelta(targetSize, duration).SetEase(ease);
        if(setUpdate){
            tweener.SetUpdate(true);
        }
        if(delay != 0 && delay > 0){
            tweener.SetDelay(delay);
        }
        return tweener;
    }

    public override void SetToStart()
    {
        rectTransform.sizeDelta = startSize;
    }

    public override void SetToTarget()
    {
        rectTransform.sizeDelta = targetSize;
    }

    public override void SwapStartAndTarget(){
        var temp = startSize;
        startSize = targetSize;
        targetSize = temp;
    }
}
