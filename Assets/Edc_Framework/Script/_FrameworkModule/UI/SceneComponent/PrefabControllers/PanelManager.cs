using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PanelManager : BaseMonoIOCComponent
{
    public RectTransform Parent_2DUI;
    public RectTransform Parent_2DUI_Hide;
    protected readonly Dictionary<Type, IBaseUIControl> createPanelContainer = new();

    /// <summary>
    /// 获取面板
    /// </summary>
    protected T GetPanel<T>()where T : IBaseUIControl
    {
        return (T)createPanelContainer[typeof(T)];
    }

    /// <summary>
    /// 打开面板(执行面板对应类相关的委托,并返回对应面板的实例)
    /// </summary>
    protected async void OpenPanel<T>(Action<T> onCreatePanel) where T : BaseUIControl
    {
        var type = typeof(T);
        await CreatePanel<T>(type);
        onCreatePanel?.Invoke(createPanelContainer[type] as T);//执行委托
        ShowPanel(type);
    }

    /// <summary>
    /// 打开面板(执行面板对应类相关的委托,并返回对应面板的实例)
    /// </summary>
    protected async void OpenPanel<T>(Action onCreatePanel) where T : BaseUIControl
    {
        var type = typeof(T);
        await CreatePanel<T>(type);
        onCreatePanel?.Invoke();//执行委托
        ShowPanel(type);
    }

    private async UniTask CreatePanel<T>(Type type) where T : BaseUIControl
    {
        if (!createPanelContainer.ContainsKey(type))
        {
            var pathInfo = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            var panelInfo = GetPanelInfo(pathInfo.Key);
            var control = Activator.CreateInstance(type) as T;
            createPanelContainer.Add(type, control);
            await ((IBaseUIControl)control).CreatePanel(panelInfo, Parent_2DUI);
        }
    }

    private void ShowPanel(Type type)
    {
        var panel = createPanelContainer[type];
        if (!panel.IsShow)
        {
            ((IBaseUIControl)panel).Open();
        }
        else
        {
            LogManager.LogWarning($"打开了一个正在显示中的UI面板 {type.Name}");
        }
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    protected void ClosePanel<T>(Action hideFinishCallBack) where T : BaseUIControl
    {
        var type = typeof(T);
        ClosePanel(type, hideFinishCallBack);
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    protected void ClosePanel(Type type, Action hideFinishCallBack)
    {
        if (createPanelContainer.TryGetValue(type, out var panel))
        {
            if (panel.IsShow)
            {
                ((IBaseUIControl)panel).Close(hideFinishCallBack);
                if (panel.IsHideFinishDestroy)
                {
                    createPanelContainer.Remove(panel.GetType());
                }
            }
            else
            {
                LogManager.LogWarning($"关闭了一个已经隐藏的UI面板 {type.Name}");
            }
        }
        else
        {
            LogManager.LogWarning($"面板 {type.Name}");
        }
    }
    
    /// <summary>
    /// 销毁关闭的面板
    /// </summary>
    protected void DestroyClosePanel<T>() where T : BaseUI
    {
        var type = typeof(T);
        if (createPanelContainer.TryGetValue(type, out var panel))
        {
            if (!panel.IsShow)
            {
                panel.DestroyPanel();
                createPanelContainer.Remove(type);
                Resources.UnloadUnusedAssets();
            }
            else
            {
                LogManager.LogWarning($"面板 {type.Name} 正在显示，无法销毁");
            }
        }
        else
        {
            LogManager.LogWarning($"需要销毁的面板 {type.Name} 没有创建或已销毁");
        }
    }

    /// <summary>
    /// 销毁所有关闭的面板
    /// </summary>
    protected void DestroyAllClosePanel(){
        var count = 0;
        var hidePanel = new List<Type>();
        foreach (var item in createPanelContainer.Keys)
        {
            if(!createPanelContainer[item].IsShow){
                hidePanel.Add(item);
                count++;
            }
        }
        for (var i = 0; i < count; i++)
        {
            var type = hidePanel[i];
            createPanelContainer[type].DestroyPanel();
            createPanelContainer.Remove(type);
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 获取预制体附带信息
    /// </summary>
    protected abstract UIPrefabInfo GetPanelInfo(string prefabName);
}
