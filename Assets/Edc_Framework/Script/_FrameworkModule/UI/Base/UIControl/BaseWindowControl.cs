using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseWindowControl : IBaseUIControl, IWindowUI { }

public class BaseWindowControl<T> : BaseUIControl<T>, IBaseWindowControl
where T : IBaseWindow
{
    void IWindowUI.Cover()
    {
        OnCover();
    }

    void IWindowUI.Reveal()
    {
        OnReveal();
    }

    protected virtual void OnCover()
    {
        ((IWindowUI)panel).Cover();
    }

    protected virtual void OnReveal()
    {
        ((IWindowUI)panel).Reveal();
    }
}
