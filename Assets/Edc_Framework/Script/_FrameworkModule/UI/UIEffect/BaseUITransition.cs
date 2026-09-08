using System;
using UnityEngine;

public abstract class BaseUITransition : MonoBehaviour
{
    public abstract void PlayShow(Action onComplete);
    public abstract void PlayHide(Action onComplete);
    public virtual void Stop() { }
}
