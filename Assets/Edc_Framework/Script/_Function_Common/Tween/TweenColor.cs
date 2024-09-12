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
    [SerializeField, HideInInspector]
    private Graphic graphic;
    [SerializeField, HideInInspector]
    private SpriteRenderer sprite;

    private void Reset()
    {
        TryGetComponent(out Graphic _image);
        TryGetComponent(out SpriteRenderer _sprite);
        if(_image != null){
            graphic = _image;
            startColor = targetColor = graphic.color;
        }
        else{
            sprite = _sprite;
            startColor = targetColor = sprite.color;
        }
    }

    private void Start() {
        SetToStart();
    }

    protected override Tweener ForwardPlay(){
        return Play(targetColor);
    }
    protected override Tweener ReversePlay(){
        return Play(startColor);
    }

    private Tweener Play(Color targetColor){
        Tweener tweener;
        if(graphic != null){
            tweener = graphic.DOColor(targetColor, duration).SetEase(ease);
        }
        else{
            tweener = sprite.DOColor(targetColor, duration).SetEase(ease);
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
        if(graphic != null){
            graphic.color = startColor;
        }
        else{
            sprite.color = startColor;
        }
    }

    public override void SetToTarget()
    {
        if(graphic != null){
            graphic.color = targetColor;
        }
        else{
            sprite.color = targetColor;
        }
    }
}
