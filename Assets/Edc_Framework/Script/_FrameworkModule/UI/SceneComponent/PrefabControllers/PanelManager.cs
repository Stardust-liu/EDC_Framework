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
    private readonly HashSet<Type> creatingPanelTypes = new();

    /// <summary>
    /// 获取面板
    /// </summary>
    protected T GetPanel<T>()where T : IBaseUIControl
    {
        return (T)createPanelContainer[typeof(T)];
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected async UniTask<T> OpenPanel<T>(Action<T> onCreatePanel) where T : BaseUIControl
    {
        var type = typeof(T);
        if (creatingPanelTypes.Contains(type))
        {
            LogManager.LogWarning($"面板 {type.Name} 正在创建，忽略重复打开");
            return null;
        }

        if (createPanelContainer.TryGetValue(type, out var control) && control.IsShow)
        {
            LogManager.LogWarning($"面板 {type.Name} 已经显示，忽略重复打开");
            return null;
        }

        try
        {
            var panel = await CreatePanel<T>(type);
            if (panel == null)
            {
                LogManager.LogError($"面板 {type.Name} 创建失败");
                return null;
            }

            onCreatePanel?.Invoke(panel);
            ShowPanel(type);
            return panel;
        }
        catch (Exception exception)
        {
            LogManager.LogError($"打开面板 {type.Name} 失败\n{exception}");
            return null;
        }
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected UniTask<T> OpenPanel<T>(Action onCreatePanel) where T : BaseUIControl
    {
        return OpenPanel<T>(_ => onCreatePanel?.Invoke());
    }

    private async UniTask<T> CreatePanel<T>(Type type) where T : BaseUIControl
    {
        if (createPanelContainer.TryGetValue(type, out var existingControl))
        {
            return existingControl as T;
        }
        if (!creatingPanelTypes.Add(type))
        {
            return null;
        }
        try
        {
            var pathInfo = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            if (pathInfo == null)
            {
                throw new Exception($"面板 {type.Name} 缺少 ResourceKeyAttribute");
            }
            var panelInfo = GetPanelInfo(pathInfo.Key);
            var control = Activator.CreateInstance(type) as T;
            if (control == null)
            {
                throw new Exception($"面板 {type.Name} 控制器创建失败");
            }
            await ((IBaseUIControl)control).CreatePanel(panelInfo, Parent_2DUI);
            createPanelContainer.Add(type, control);
            return control;
        }
        finally
        {
            creatingPanelTypes.Remove(type);
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
                ((IBaseUIControl)panel).Close(() =>
                {
                    try
                    {
                        hideFinishCallBack?.Invoke();
                    }
                    catch (Exception exception)
                    {
                        LogManager.LogError($"面板 {type.Name} 关闭回调执行失败\n{exception}");
                    }
                    finally
                    {
                        if (panel.IsHideFinishDestroy)
                        {
                            DestroyClosePanel(type);
                        }
                    }
                });
            }
            else
            {
                if (panel.IsHideFinish)
                {
                    LogManager.LogWarning($"重复关闭了一个已经隐藏的UI面板 {type.Name}");
                }
                else
                {
                    LogManager.LogWarning($"重复关闭了一个正在隐藏的UI面板 {type.Name}");
                }
            }
        }
    }
    
    /// <summary>
    /// 销毁关闭的面板
    /// </summary>
    protected void DestroyClosePanel<T>() where T : BaseUI
    {
        var type = typeof(T);
        DestroyClosePanel(type);
    }

    /// <summary>
    /// 销毁关闭的面板
    /// </summary>
    protected void DestroyClosePanel(Type type)
    {
        if (createPanelContainer.TryGetValue(type, out var panel))
        {
            if (panel.IsHideFinish)
            {
                panel.DestroyPanel();
                createPanelContainer.Remove(type);
            }
            else
            {
                LogManager.LogWarning($"面板 {type.Name} 正在显示，或未关闭完成，无法销毁");
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
            if(!createPanelContainer[item].IsHideFinish){
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
