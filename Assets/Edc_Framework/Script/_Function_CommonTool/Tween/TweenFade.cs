using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


public class TweenFade : BaseTween
{
    public float startFade;
    public float targetFade;
    public CanvasGroup canvasGroup;

    private void Reset()
    {
        TryGetComponent(out CanvasGroup _canvasGroup);
        if(_canvasGroup != null){
            canvasGroup = _canvasGroup;
            startFade = targetFade = canvasGroup.alpha;
        }
    }

    protected override Tweener ForwardPlay(){
        return Play(targetFade);
    }
    protected override Tweener ReversePlay(){
        return Play(startFade);
    }

    private Tweener Play(float targetFade){
        Tweener tweener;
        tweener = canvasGroup.DOFade(targetFade, duration).SetEase(ease);
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
        canvasGroup.alpha = startFade;
    }

    public override void SetToTarget()
    {
        canvasGroup.alpha = targetFade;
    }

    public override void SwapStartAndTarget(){
        var temp = startFade;
        startFade = targetFade;
        targetFade = temp;
    }
}
