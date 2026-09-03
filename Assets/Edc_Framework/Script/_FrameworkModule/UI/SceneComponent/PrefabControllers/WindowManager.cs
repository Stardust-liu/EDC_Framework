using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : PanelManager
{
    public Image windowMaskPanel; 
    private IBaseWindowControl currentWindow;
    private Stack<IBaseWindowControl> openWindowStack;
    public WindowSetting WindowSetting {get; private set;}

    protected override void Init(){
        base.Init();
        openWindowStack = new Stack<IBaseWindowControl>();
    }

    protected override void Ready()
    {
        base.Ready();
        WindowSetting = Hub.FrameworkConfig.Get<WindowSetting>("WindowSetting");
        WindowSetting.Init();
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    public void OpenWindow<T>(Action<T> onCreatePanel = null)
    where T : BaseUIControl, IBaseWindowControl
    {
        OpenWindowAsync(onCreatePanel).Forget();
    }

    private async UniTask OpenWindowAsync<T>(Action<T> onCreatePanel) where T : BaseUIControl, IBaseWindowControl
    {
        var newWindow = await OpenPanel(onCreatePanel);
        if (newWindow == null)
        {
            return;
        }

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
        currentWindow?.Cover();
        windowStack.Push(window);
        currentWindow = windowStack.Peek();
        windowMask.enabled = true;
        windowMask.transform.SetSiblingIndex(windowStack.Count - 1);
    }

    private void CloseWindow(Stack<IBaseWindowControl> windowStack, Image windowMask)
    {
        if (windowStack == null || windowStack.Count == 0)
        {
            LogManager.LogWarning("当前没有可关闭的窗口");
            if (windowMask != null)
            {
                windowMask.enabled = false;
            }
            currentWindow = null;
            return;
        }
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
            currentWindow.Reveal();
            windowMask.transform.SetSiblingIndex(windowStack.Count - 1);
        }
    }

    protected override UIPrefabInfo GetPanelInfo(string prefabName)
    {
        return WindowSetting.GetPanelInfo(prefabName);
    }
}
