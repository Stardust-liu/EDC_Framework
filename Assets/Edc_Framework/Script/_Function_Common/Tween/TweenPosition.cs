using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenPosition : BaseTween
{
    public bool isWorldPos;
    public Vector3 startPos;
    public Vector3 targetPos;
    public RectTransform rectTransform;

    private void Reset()
    {
        TryGetComponent(out RectTransform _rectTransform);
        if(_rectTransform != null){
            rectTransform = _rectTransform;
            if(isWorldPos){
                startPos = targetPos = rectTransform.position;
            }
            else{
                startPos = targetPos = rectTransform.localPosition;
            }
        }
    }

    protected override Tweener ForwardPlay(){
        return Play(targetPos);
    }
    protected override Tweener ReversePlay(){
        return Play(startPos);
    }

    private Tweener Play(Vector3 targetPos){
        Tweener tweener;
        
        if(isWorldPos){
            tweener = rectTransform.DOMove(targetPos, duration).SetEase(ease);
        }
        else{
            tweener = rectTransform.DOAnchorPos(targetPos, duration).SetEase(ease);
        }
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
        if(isWorldPos){
            rectTransform.position = startPos;
        }
        else{
            rectTransform.anchoredPosition = startPos;
        }
    }

    public override void SetToTarget()
    {
        if(isWorldPos){
            rectTransform.position = targetPos;
        }
        else{
            rectTransform.anchoredPosition = targetPos;
        }
    }

    public override void SwapStartAndTarget(){
        var temp = startPos;
        startPos = targetPos;
        targetPos = temp;
    }
}