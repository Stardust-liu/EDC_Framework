using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : PanelManager
{
    public Image windowMaskPanel; 
    public Image windowMaskPanel_3D; 
    public Stack<BaseWindow> openWindowStack;
    public Stack<BaseWindow> openWindowStack_3D;
    public WindowSetting WindowSetting {get; private set;}

    protected override void Init(){
        base.Init();
        var resourcePath = new ResourcePath("WindowSetting","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/WindowSetting.asset");
        WindowSetting = Hub.Resources.GetScriptableobject<WindowSetting>(resourcePath);
        openWindowStack = new Stack<BaseWindow>();
        openWindowStack_3D = new Stack<BaseWindow>();
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    public void OpenWindow<T>() where T : BaseWindow
    {
        OpenPanel<T>();
        var newWindow = GetPanel<T>();
        if(!newWindow.Is3DUI){
            OpenWindow(newWindow, openWindowStack, windowMaskPanel);
        }
        else{
            OpenWindow(newWindow, openWindowStack_3D, windowMaskPanel_3D);
        }
    }

    /// <summary>
    /// 关闭最上层窗口
    /// </summary>
    public void CloseWindow(Action hideFinishCallBack = null){
        CloseWindow(openWindowStack, windowMaskPanel, hideFinishCallBack);
    }

    /// <summary>
    /// 关闭最上层窗口
    /// </summary>
    public void CloseWindow_3D(Action hideFinishCallBack = null){
        CloseWindow(openWindowStack_3D, windowMaskPanel_3D, hideFinishCallBack);
    }


    private void OpenWindow(BaseWindow window, Stack<BaseWindow> windowStack, Image windowMask){
        windowStack.Push(window);
        windowMask.enabled = true;
        windowMask.transform.SetSiblingIndex(windowStack.Count-1);
    }

    private void CloseWindow(Stack<BaseWindow> windowStack, Image windowMask ,Action hideFinishCallBack){
        ClosePanel(windowStack.Pop().GetType(), hideFinishCallBack);
        if (windowStack.Count == 0)
        {
            windowMask.enabled = false;
        }
        else
        {
            windowMask.transform.SetSiblingIndex(windowStack.Count - 1);
        }
    }

    protected override UIPrefabInfo GetPanelInfo(string prefabName)
    {
        return WindowSetting.GetPanelInfo(prefabName);
    }
}
