using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : PanelManager
{
    public Image windowMaskPanel; 
    public Image windowMaskPanel_3D; 
    private IBaseWindowControl currentWindow;
    private Stack<IBaseWindowControl> openWindowStack;
    public WindowSetting WindowSetting {get; private set;}

    protected override void Init(){
        base.Init();
        var resourcePath = new ResourcePath("WindowSetting","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/WindowSetting.asset");
        WindowSetting = Hub.Resources.GetScriptableobject<WindowSetting>(resourcePath);
        openWindowStack = new Stack<IBaseWindowControl>();
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    public void OpenWindow<T>(Action<T> onOpenInit = null)
    where T : BaseUIControl, IBaseWindowControl
    {
        OpenPanel(onOpenInit);
        var newWindow = GetPanel<T>();
        OpenWindow(newWindow, openWindowStack, windowMaskPanel);
    }

    /// <summary>
    /// 关闭最上层窗口
    /// </summary>
    public void CloseWindow()
    {
        CloseWindow(openWindowStack, windowMaskPanel);
    }

    /// <summary>
    /// 检查当前打开的窗口是否是指定类型
    /// </summary>
    public bool CheckCurrentWindow<T>() where T : IBaseWindowControl
    {
        return currentWindow is T;
    }

    private void OpenWindow(IBaseWindowControl window, Stack<IBaseWindowControl> windowStack, Image windowMask)
    {
        windowStack.Push(window);
        currentWindow = windowStack.Peek();
        windowMask.enabled = true;
        windowMask.transform.SetSiblingIndex(windowStack.Count - 1);
    }

    private void CloseWindow(Stack<IBaseWindowControl> windowStack, Image windowMask)
    {
        //如果关闭后要做什么操作直接重写面板自身的HideFinish就行
        ClosePanel(windowStack.Pop().GetType(), null);
        if (windowStack.Count == 0)
        {
            currentWindow = null;
            windowMask.enabled = false;
        }
        else
        {
            currentWindow = windowStack.Peek();
            windowMask.transform.SetSiblingIndex(windowStack.Count - 1);
        }
    }

    protected override UIPrefabInfo GetPanelInfo(string prefabName)
    {
        return WindowSetting.GetPanelInfo(prefabName);
    }
}
