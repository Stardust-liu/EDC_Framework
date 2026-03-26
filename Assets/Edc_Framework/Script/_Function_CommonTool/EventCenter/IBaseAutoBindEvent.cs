using System;
using UnityEngine;

public static class BaseAutoBindEvent 
{
    public static void SetBinder_DisableRemoveListenerr(this IBaseAutoBindEvent bindEvent, Action addListener, Action removeListener, EventAutoBinder eventAutoBinder){
        eventAutoBinder.RegisterEnableAddListener(addListener);
        eventAutoBinder.RegisterDisableRemoveListener(removeListener);
    }

    public static void SetBinder_DestroyRemoveListener(this IBaseAutoBindEvent bindEvent, Action removeListener, EventAutoBinder eventAutoBinder){
        eventAutoBinder.RegisterDestroyRemoveListener(removeListener);
    }

    public static EventAutoBinder GetEventAutoBinder(this IBaseAutoBindEvent bindEvent, GameObject bindObj){
        if(!bindObj.TryGetComponent<EventAutoBinder>(out var eventAutoBinder))
        {
            eventAutoBinder = bindObj.AddComponent<EventAutoBinder>();
        }
        return eventAutoBinder;
    }
}

public interface IBaseAutoBindEvent{}
