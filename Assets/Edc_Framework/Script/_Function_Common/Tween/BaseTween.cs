using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
public abstract class BaseTween : MonoBehaviour
{
    [LabelText("Tween时间"), SerializeField]
    protected float duration = WaitTime.slow;
    [LabelText("延迟时间"), SerializeField]
    protected float delay = 0;
    public Ease ease = Ease.OutQuad;
    public bool setUpdate;
    private Tweener tweener;
    public Tweener Play(bool isForwardPlay = true){
        if(isForwardPlay){
            tweener = ForwardPlay();
        }
        else{
            tweener = ReversePlay();
        }
        return tweener;
    }

    public void Pause(){
        tweener?.Pause();
    }

    protected abstract Tweener ForwardPlay();
    protected abstract Tweener ReversePlay();

    [Button("SetToStart")]
    public abstract void SetToStart();
    [Button("SetToTarget")]
    public abstract void SetToTarget();

    public void SetDuration(float _duration){
        duration = _duration;
    }

    public void SetDelay(float _delay){
        delay = _delay;
    }
    
    public float GetDuration(){
        return duration;
    }

    public float GetDelay(){
        return delay;
    }
}
