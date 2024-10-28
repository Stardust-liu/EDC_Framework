using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI_V<Presenter> : MonoBehaviour, IBaseUI_V  where Presenter : BaseUI_P
{
    protected Presenter presenter;
    public TweenGroup tweenGroupIn;
    public TweenGroup tweenGroupOut;
    protected virtual void Awake() {}
    protected virtual void Start() {}
    protected virtual void OnDestroy() {}
    public virtual void StartShow() 
    {
        UpdateMargins();
        UIController.eventCenter.AddListener(UIControllerEventName.UpdateMargins, UpdateMargins);
    }
    public virtual void StartHide() 
    {
        UIController.eventCenter.RemoveListener(UIControllerEventName.UpdateMargins, UpdateMargins);
    }
    public virtual void ShowFinish() {}
    public virtual void HideFinish() {}
    protected virtual void UpdateMargins(){}

    void IBaseUI_V.Destroy()
    {
        Destroy(gameObject);
    }

    void IBaseUI_V.Init(BaseUI_P _presenter)
    {
        presenter =(Presenter) _presenter;
    }
}
