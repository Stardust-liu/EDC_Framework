using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenRotation : BaseTween
{
    public bool isWorldPos;
    public Vector3 startRotation;
    public Vector3 targetRotation;
    public Transform objectTransform;

    private void Reset()
    {
        TryGetComponent(out Transform _objectTransform);
        if(_objectTransform != null){
            objectTransform = _objectTransform;
            if(isWorldPos){
                startRotation = targetRotation = objectTransform.rotation.eulerAngles;
            }
            else{
                startRotation = targetRotation = objectTransform.localRotation.eulerAngles;
            }
        }
    }

    protected override Tweener ForwardPlay(){
        return Play(targetRotation);
    }
    protected override Tweener ReversePlay(){
        return Play(startRotation);
    }

    private Tweener Play(Vector3 targetRotation){
        Tweener tweener;
        if(isWorldPos){
            tweener = objectTransform.DORotate(targetRotation, duration).SetEase(ease);
        }
        else{
            tweener = objectTransform.DOLocalRotate(targetRotation, duration).SetEase(ease);
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
            objectTransform.rotation = Quaternion.Euler(startRotation);
        }
        else{
            objectTransform.localRotation = Quaternion.Euler(startRotation);
        }
    }

    public override void SetToTarget()
    {
        if(isWorldPos){
            objectTransform.rotation = Quaternion.Euler(targetRotation);
        }
        else{
            objectTransform.localRotation = Quaternion.Euler(targetRotation);
        }
    }

    public override void SwapStartAndTarget(){
        var temp = startRotation;
        startRotation = targetRotation;
        targetRotation = temp;
    }
}