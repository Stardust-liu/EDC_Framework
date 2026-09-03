using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ViewManager : BaseViewManager
{
    private IBaseViewControl currentView;
    private List<IBaseViewControl> openViewInfo;
    private Stack<IBaseViewControl> openViewStack;
    public ViewSetting ViewSetting { get; private set; }

    protected override void Init()
    {
        base.Init();
        openViewInfo = new List<IBaseViewControl>();
        openViewStack = new Stack<IBaseViewControl>();
    }

    protected override void Ready()
    {
        base.Ready();
        ViewSetting = Hub.FrameworkConfig.Get<ViewSetting>("ViewSetting");
        ViewSetting.Init();
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>(Action<T> onCreatePanel) 
    where T : BaseUIControl, IBaseViewControl
    {
        if (currentView != null)
        {
            var closePanelType = currentView.GetType();
            ClosePanel(closePanelType, OpenView);
            void OpenView()
            {
                OpenView<T>(onCreatePanel);
            }
        }
        else
        {
            OpenView<T>(onCreatePanel);
        }
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>() 
    where T : BaseUIControl, IBaseViewControl
    {
        ChangeView<T>((Action)null);
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>(Action onCreatePanel) 
    where T : BaseUIControl, IBaseViewControl
    {
        if (currentView != null)
        {
            var closePanelType = currentView.GetType();
            ClosePanel(closePanelType, OpenView);
            void OpenView()
            {
                OpenView<T>(onCreatePanel);
            }
        }
        else
        {
            OpenView<T>(onCreatePanel);
        }
    }

    /// <summary>
    /// 返回上一个视图页面
    /// </summary>
    public void BackLastView(Action lasetViewOnCreate = null)
    {
        if (BackLastView(out Type closePanelType))
        {
            ClosePanel(closePanelType, WaitOpenView);
        }
        async void WaitOpenView()
        {
            //照搬了PanelManager OpenPanel类的逻辑，并进行了部分修改
            var type = currentView.GetType();
            await CreatePanel(type, currentView);
            lasetViewOnCreate?.Invoke();//执行委托
            var panel = createPanelContainer[type];
            panel.Open();
        }
    }

    /// <summary>
    /// 检查当前打开的界面是否是指定类型
    /// </summary>
    public bool CheckCurrentView<T>() where T : IBaseViewControl
    {
        if (currentView == null)
        {
           return false; 
        }
        else
        {
            return currentView is T;
        }
    }

    private bool BackLastView(out Type closePanelType)
    {
        if (openViewStack.Count > 1)
        {
            openViewInfo.Remove(openViewStack.Pop());
            closePanelType = currentView.GetType();
            currentView = openViewStack.Peek();
            return true;
        }
        else
        {
            LogManager.LogError("没有上一个页面");
            closePanelType = null;
            return false;
        }
    }

    private void OpenView<T>(Action<T> onCreatePanel) where T : BaseUIControl, IBaseViewControl
    {
        OpenViewAsync(onCreatePanel).Forget();
    }

    private async UniTask OpenViewAsync<T>(Action<T> onCreatePanel) where T : BaseUIControl, IBaseViewControl
    {
        var viewControl = await OpenPanel(onCreatePanel);
        if (viewControl == null)
        {
            return;
        }

        OpenView(viewControl);
    }

    private void OpenView<T>(Action onCreatePanel) where T : BaseUIControl, IBaseViewControl
    {
        OpenViewAsync<T>(onCreatePanel).Forget();
    }

    private async UniTask OpenViewAsync<T>(Action onCreatePanel) where T : BaseUIControl, IBaseViewControl
    {
        var viewControl = await OpenPanel<T>(onCreatePanel);
        if (viewControl == null)
        {
            return;
        }

        OpenView(viewControl);
    }

    private void OpenView(IBaseViewControl viewControl)
    {
        currentView = viewControl;
        if (openViewInfo.Contains(currentView))
        {
            while (openViewStack.Peek() != currentView)
            {
                openViewInfo.Remove(openViewStack.Pop());
            }
        }
        else
        {
            openViewStack.Push(currentView);
            openViewInfo.Add(currentView);
        }
    }

    private async UniTask CreatePanel(Type type, IBaseViewControl baseViewControl)
    {
        if (!createPanelContainer.ContainsKey(type))
        {
            var pathInfo = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            var panelInfo = GetPanelInfo(pathInfo.Key);
            await baseViewControl.CreatePanel(panelInfo, Parent_2DUI);
            createPanelContainer.Add(type, baseViewControl);
        }
    }

    protected override UIPrefabInfo GetPanelInfo(string prefabName)
    {
        return ViewSetting.GetPanelInfo(prefabName);
    }
}
