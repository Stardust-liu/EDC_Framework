using System;
using UnityEngine;

public class EventAutoBinder : MonoBehaviour
{
    private Action enableAddListener;
    private Action disableRemoveListener;
    private Action DestroyRemoveListener;

    public void RegisterEnableAddListener(Action action)
    {
        enableAddListener += action;
        action.Invoke();
        //物体自动添加EventAutoBinder组件后，由于已经调用过OnEnable，
        //因此需要强制调用一次添加事件监听的方法，否则触发事件时，绑定的方法不会调用
    }
    
    public void RegisterStartAddListener(Action action)
    {
        action.Invoke();
    }

    public void RegisterDisableRemoveListener(Action action)
    {
        disableRemoveListener += action;
    }

    public void RegisterDestroyRemoveListener(Action action){
        DestroyRemoveListener += action;
    }

    void OnEnable() {
        enableAddListener?.Invoke();
    }

    void OnDisable(){
        disableRemoveListener?.Invoke();
    }


    void OnDestroy() {
        DestroyRemoveListener?.Invoke();
    }
}
