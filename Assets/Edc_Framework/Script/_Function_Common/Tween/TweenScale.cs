using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenScale : BaseTween
{
    public Vector3 startScale;
    public Vector3 targetScale;
    [SerializeField, HideInInspector]
    private Transform objectTransform;

    private void Reset()
    {
        TryGetComponent(out Transform _objectTransform);
        objectTransform = _objectTransform;
        startScale = targetScale = objectTransform.localScale;
    }

     private void Start() {
        SetToStart();
    }

    protected override Tweener ForwardPlay(){
        return Play(targetScale);
    }
    protected override Tweener ReversePlay(){
        return Play(startScale);
    }

    private Tweener Play(Vector3 targetScale){
        Tweener tweener;
        tweener = objectTransform.DOScale(targetScale, duration).SetEase(ease);
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
        objectTransform.localScale = startScale;
    }

    public override void SetToTarget()
    {
        objectTransform.localScale = targetScale;
    }
}
