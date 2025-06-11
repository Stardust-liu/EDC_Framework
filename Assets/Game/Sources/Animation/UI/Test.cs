using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Test : BaseTest
{
    public int a;

    private void Open(){
        Debug.Log("调用Open");
    }
    public virtual void Open1(){
        Debug.Log("调用Open1");
    }

    protected override void Open2()
    {
        Debug.Log("调用Open2");
    }
}

public abstract class BaseTest : MonoBehaviour{
    protected abstract void Open2();
}
