using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI_V : MonoBehaviour, IBaseUI_V
{
    protected BaseUI_P presenter;

    protected virtual void Awake() {}
    protected virtual void Start() {}
    protected virtual void OnEnable() {}
    protected virtual void OnDisable() {}
    protected virtual void OnDestroy() {}
    void IBaseUI_V.Destroy()
    {
        Destroy(gameObject);
    }

    void IBaseUI_V.Init(BaseUI_P _presenter)
    {
        presenter = _presenter;
    }
}
