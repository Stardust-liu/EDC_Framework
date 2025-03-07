using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


public class TweenColor : BaseTween
{
    public Color startColor;
    public Color targetColor;
    public Graphic graphic;

    private void Reset()
    {
        TryGetComponent(out Graphic _image);
        if(_image != null){
            graphic = _image;
            startColor = targetColor = graphic.color;
        }
    }

    protected override Tweener ForwardPlay(){
        return Play(targetColor);
    }
    protected override Tweener ReversePlay(){
        return Play(startColor);
    }

    private Tweener Play(Color targetColor){
        Tweener tweener;
        tweener = graphic.DOColor(targetColor, duration).SetEase(ease);
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
        graphic.color = startColor;
    }

    public override void SetToTarget()
    {
        graphic.color = targetColor;
    }

    public override void SwapStartAndTarget(){
        var temp = startColor;
        startColor = targetColor;
        targetColor = temp;
    }
}
