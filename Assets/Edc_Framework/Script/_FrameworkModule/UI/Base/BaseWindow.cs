using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaseWindowExtensions{
    /// <summary>
    /// 关闭最上层窗口
    /// </summary>
    public static void CloseTopWindow(this IBaseWindow baseViewExtensions){
        if(baseViewExtensions.Is3DUI){
            Hub.Window.CloseWindow_3D();
        }
        else{
            Hub.Window.CloseWindow();
        }
    }
}

public interface IBaseWindow : IUIType{}

public class BaseWindow<Model> : BaseUI<Model>, IBaseWindow where Model : BaseUI_Model, new(){}

public class BaseWindow: BaseUI, IBaseWindow{}