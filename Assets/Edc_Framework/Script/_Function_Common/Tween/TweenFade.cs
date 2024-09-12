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
    [SerializeField, HideInInspector]
    private CanvasGroup canvasGroup;
    [SerializeField, HideInInspector]
    private Graphic graphic;
    [SerializeField, HideInInspector]
    private SpriteRenderer sprite;

    private void Reset()
    {
        TryGetComponent(out CanvasGroup _canvasGroup);
        TryGetComponent(out Graphic _graphic);
        TryGetComponent(out SpriteRenderer _sprite);
        if(_canvasGroup != null){
            canvasGroup = _canvasGroup;
            startFade = targetFade = canvasGroup.alpha;
        }
        else if(_graphic != null){
            graphic = _graphic;
            startFade = targetFade = graphic.color.a;
        }
        else{
            sprite = _sprite;
            startFade = targetFade = sprite.color.a;
        }
    }

    private void Start() {
        SetToStart();
    }

    protected override Tweener ForwardPlay(){
        return Play(targetFade);
    }
    protected override Tweener ReversePlay(){
        return Play(startFade);
    }

    private Tweener Play(float targetFade){
        Tweener tweener;
        if(canvasGroup != null){
            tweener = canvasGroup.DOFade(targetFade, duration).SetEase(ease);
        }
        else if(graphic != null){
            tweener = graphic.DOFade(targetFade, duration).SetEase(ease);
        }
        else{
            tweener = sprite.DOFade(targetFade, duration).SetEase(ease);
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
        if(canvasGroup != null){
            canvasGroup.alpha = startFade;
        }
        else if(graphic != null){
            var color = graphic.color;
            graphic.color = new Color(color.r, color.g, color.b, startFade);
        }
        else{
            var color = sprite.color;
            sprite.color = new Color(color.r, color.g, color.b, startFade);
        }
    }

    public override void SetToTarget()
    {
        if(canvasGroup != null){
            canvasGroup.alpha = targetFade;
        }
        else if(graphic != null){
            var color = graphic.color;
            graphic.color = new Color(color.r, color.g, color.b, targetFade);
        }
        else{
            var color = sprite.color;
            sprite.color = new Color(color.r, color.g, color.b, targetFade);
        }
    }
}
