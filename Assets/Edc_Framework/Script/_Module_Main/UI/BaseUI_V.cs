using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI_V : MonoBehaviour
{
    private void OnEnable() {
        OnActive();
    }
    private void Start() {
        OnStart();
    }

    protected virtual void OnStart(){}
    protected virtual void OnActive(){}
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
