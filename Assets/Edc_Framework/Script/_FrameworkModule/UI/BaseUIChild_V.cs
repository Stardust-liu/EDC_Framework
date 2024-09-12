using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIChild_V : MonoBehaviour, IBaseUIChild
{
    protected BaseUI_P presenter;

    protected virtual void Start() {}
    protected virtual void OnEnable() {}
    protected virtual void OnDisable() {}
    protected virtual void OnDestroy() {}

    void IBaseUIChild.Init(BaseUI_P _presenter)
    {
        presenter = _presenter;
    }
}
