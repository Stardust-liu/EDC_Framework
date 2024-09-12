using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window_MVP<T1, T2, T3>: Window_VP<T2, T3>
where T1 : class, new()
where T2 : BaseUI_V
where T3 : BaseWindow_P
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


public abstract class Window_VP<T2, T3>:BaseWindow_P
where T2 : BaseUI_V
where T3 : BaseWindow_P
{
    protected T2 window_V;
    private static T3 instance;
    public static T3 Instance 
    {
        get 
        {
            instance ??= WindowManager.GetWindow<T3>(typeof(T3).Name);
            return instance;
        }
    }
    public override void Destroy(){
        base.Destroy();
        ((IBaseUI_V)window_V).Destroy();
        Hub.Window.DestroyWindow(typeof(T3).Name);
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    protected void CreateUiPrefab(string name){
        var prefab = GameObject.Instantiate(SetPrefabInfo(name), WindowManager.Parent);
        isCreate = true;
        window_V = prefab.GetComponent<T2>();
        ((IBaseUI_V)window_V).Init(Instance);
    }

}

public abstract class BaseWindow_P : BaseUI_P
{
    protected GameObject SetPrefabInfo(string name){
        var prefabInfo = FrameworkManager.Window.GetPrefabInfo(name);
        isHideFinishDestroy = prefabInfo.isHideFinishDestroy;
        return prefabInfo.prefab;
    }
}
