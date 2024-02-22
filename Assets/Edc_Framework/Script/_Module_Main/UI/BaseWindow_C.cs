using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window_MVC<T1, T2, T3>: Window_VC<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BaseWindow_C
{
    protected static T1 window_M;
    public override void CreateUiPrefab(){
        window_M = Activator.CreateInstance<T1>();
    }
    public override void Destroy(){
        base.Destroy();
        window_M = null;
    }
}


public abstract class Window_VC<T2, T3>:BaseWindow_C
where T2 : BaseUI_V
where T3 : BaseWindow_C
{
    protected T2 window_V;
    public static T3 Instance {get {return WindowManager.GetWindow<T3>(typeof(T3).Name);}}
    public override void Destroy(){
        base.Destroy();
        window_V.Destroy();
        Hub.Window.DestroyWindow(typeof(T3).Name);
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    protected void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name), WindowManager.Parent);
        isCreate = true;
        window_V = prefab.GetComponent<T2>();
    }

}

public abstract class BaseWindow_C : BaseUI_C
{
    protected GameObject SetPrefabInfo(string name){
        var prefabInfo = FrameworkManager.Window.GetPrefabInfo(name);
        isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        return prefabInfo.prefab;
    }
}
