using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenPosition : BaseTween
{
    public bool isWorldPos;
    public Vector3 startPos;
    public Vector3 targetPos;
    [SerializeField, HideInInspector]
    private Transform objectTransform;

    [SerializeField, HideInInspector]
    private RectTransform rectTransform;

    private void Reset()
    {
        TryGetComponent(out Transform _objectTransform);
        TryGetComponent(out RectTransform _rectTransform);
        if(_objectTransform != null){
            objectTransform = _objectTransform;
            if(isWorldPos){
                startPos = targetPos = objectTransform.position;
            }
            else{
                startPos = targetPos = objectTransform.localPosition;
            }
        }
        else{
            rectTransform = _rectTransform;
            if(isWorldPos){
                startPos = targetPos = rectTransform.position;
            }
            else{
                startPos = targetPos = rectTransform.localPosition;
            }
        }
    }

     private void Start() {
        SetToStart();
    }

    protected override Tweener ForwardPlay(){
        return Play(targetPos);
    }
    protected override Tweener ReversePlay(){
        return Play(startPos);
    }

    private Tweener Play(Vector3 targetPos){
        Tweener tweener;
        if(objectTransform != null){
            if(isWorldPos){
                tweener = objectTransform.DOMove(targetPos, duration).SetEase(ease);
            }
            else{
                tweener = objectTransform.DOLocalMove(targetPos, duration).SetEase(ease);
            }
        }
        else{
            if(isWorldPos){
                tweener = rectTransform.DOMove(targetPos, duration).SetEase(ease);
            }
            else{
                tweener = rectTransform.DOAnchorPos(targetPos, duration).SetEase(ease);
            }
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
        if(objectTransform != null){
            if(isWorldPos){
                objectTransform.position = startPos;
            }
            else{
                objectTransform.localPosition = startPos;
            }
        }
        else{
            if(isWorldPos){
                rectTransform.position = startPos;
            }
            else{
                rectTransform.anchoredPosition = startPos;
            }
        }
    }

    public override void SetToTarget()
    {
        if(objectTransform != null){
            if(isWorldPos){
                objectTransform.position = targetPos;
            }
            else{
                objectTransform.localPosition = targetPos;
            }
        }
        else{
            if(isWorldPos){
                rectTransform.position = targetPos;
            }
            else{
                rectTransform.anchoredPosition = targetPos;
            }
        }
    }
}