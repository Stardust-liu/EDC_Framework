using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : BaseViewManager
{
    private IBaseViewControl currentView;
    private HashSet<IBaseViewControl> openViewInfo;
    private Stack<IBaseViewControl> openViewStack;
    public ViewSetting ViewSetting { get; private set; }


    protected override void Init()
    {
        base.Init();
        var resourcePath = new ResourcePath("ViewSetting", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/ViewSetting.asset");
        ViewSetting = Hub.Resources.GetScriptableobject<ViewSetting>(resourcePath);
        openViewInfo = new HashSet<IBaseViewControl>();
        openViewStack = new Stack<IBaseViewControl>();
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>(Action<T> onOpenInit)
    where T : BaseUIControl, IBaseViewControl
    {
        if (currentView != null)
        {
            var closePanelType = currentView.GetType();
            ClosePanel(closePanelType, OpenView);
            void OpenView()
            {
                OpenView<T>(onOpenInit);
            }
        }
        else
        {
            OpenView<T>(onOpenInit);
        }
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>()
    where T : BaseUIControl, IBaseViewControl
    {
        ChangeView<T>(null, true);
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>(Action onOpenInit, bool dummyValue = true)
    where T : BaseUIControl, IBaseViewControl
    {
        if (currentView != null)
        {
            var closePanelType = currentView.GetType();
            ClosePanel(closePanelType, OpenView);
            void OpenView()
            {
                OpenView<T>(onOpenInit);
            }
        }
        else
        {
            OpenView<T>(onOpenInit);
        }
    }

    /// <summary>
    /// 返回上一个视图页面
    /// </summary>
    public void BackLastView(Action onOpenInit = null)
    {
        if (BackLastView(out Type closePanelType))
        {
            ClosePanel(closePanelType, WaitOpenView);
        }
        void WaitOpenView()
        {
            //照搬了PanelManager OpenPanel类的逻辑，并进行了部分修改
            var type = currentView.GetType();
            CreatePanel(type, currentView);
            onOpenInit?.Invoke();//执行委托
            var panel = createPanelContainer[type];
            panel.Open();
        }
    }

    /// <summary>
    /// 检查当前打开的界面是否是指定类型
    /// </summary>
    public bool CheckCurrentView<T>() where T : BaseView
    {
        return currentView is T;
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

    private void OpenView<T>(Action<T> onOpened) where T : BaseUIControl, IBaseViewControl
    {
        OpenPanel(onOpened);
        OpenView<T>();
    }

    private void OpenView<T>(Action onOpened) where T : BaseUIControl, IBaseViewControl
    {
        OpenPanel<T>(onOpened);
        OpenView<T>();
    }

    private void OpenView<T>() where T : IBaseViewControl
    {
        currentView = GetPanel<T>();
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

    private void CreatePanel(Type type, IBaseViewControl baseViewControl)
    {
        if (!createPanelContainer.ContainsKey(type))
        {
            var pathInfo = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            var panelInfo = GetPanelInfo(pathInfo.Key);
            baseViewControl.CreatePanel(panelInfo, Parent_2DUI, Parent_3DUI);
            createPanelContainer.Add(type, baseViewControl);
        }
    }

    protected override UIPrefabInfo GetPanelInfo(string prefabName)
    {
        return ViewSetting.GetPanelInfo(prefabName);
    }
}
