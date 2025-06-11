using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PanelManager : BaseMonoIOCComponent
{
    public RectTransform Parent_3DUI;
    public RectTransform Parent_2DUI;
    private readonly Dictionary<Type, BaseUI> createPabelContainer = new();

    /// <summary>
    /// 获取面板
    /// </summary>
    protected T GetPanel<T>() where T : BaseUI
    {
        return (T)createPabelContainer[typeof(T)];
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected void OpenPanel<T>() where T : BaseUI
    {
        var type = typeof(T);
        OpenPanel(type);
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected void OpenPanel(Type type){
        if(!createPabelContainer.ContainsKey(type)){
            var pathInfo = (ResourceKeyAttribute)Attribute.GetCustomAttribute(type, typeof(ResourceKeyAttribute));
            var panelInfo =  GetPanelInfo(pathInfo.Key);
            var parent = panelInfo.is3DUI? Parent_3DUI : Parent_2DUI;
            var container = GameObject.Instantiate(panelInfo.prefab, parent).GetComponent<BaseUI>();
            ((IBaseUI)container).Init(panelInfo);
            createPabelContainer.Add(type, container);
        }
        var panel = createPabelContainer[type];
        if(!panel.IsShow){
            ((IBaseUI)panel).Open();
        }
        else{
            LogManager.LogWarning($"打开了一个正在显示中的UI面板 {type.Name}");
        }
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    protected void ClosePanel<T>(Action hideFinishCallBack) where T : BaseUI
    {
        var type = typeof(T);
        ClosePanel(type, hideFinishCallBack);
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    protected void ClosePanel(Type type, Action hideFinishCallBack)
    {
        if(createPabelContainer.TryGetValue(type, out var panel)){
            if(panel.IsShow){
                ((IBaseUI)panel).Close(hideFinishCallBack);
            }
            else{
                LogManager.LogWarning($"关闭了一个已经隐藏的UI面板 {type.Name}");
            }
        }
        else{
            LogManager.LogWarning($"面板 {type.Name}");
        }
    }

    /// <summary>
    /// 销毁关闭的面板
    /// </summary>
    protected void DestroyClosePanel<T>() where T : BaseUI
    {  
        var type = typeof(T);
        if(createPabelContainer.TryGetValue(type, out var panel)){
            if(!panel.IsShow){
                panel.OnDestroy();
                createPabelContainer.Remove(type);
                Resources.UnloadUnusedAssets();
            }
            else{
                LogManager.LogWarning($"面板 {type.Name} 正在显示，无法销毁");
            }
        }
        else{
            LogManager.LogWarning($"需要销毁的面板 {type.Name} 没有创建或已销毁");
        }
    }

    /// <summary>
    /// 销毁所有关闭的面板
    /// </summary>
    protected void DestroyAllClosePanel(){
        var count = 0;
        var hidePanel = new List<Type>();
        foreach (var item in createPabelContainer.Keys)
        {
            if(!createPabelContainer[item].IsShow){
                hidePanel.Add(item);
                count++;
            }
        }
        for (var i = 0; i < count; i++)
        {
            var type = hidePanel[i];
            createPabelContainer[type].OnDestroy();
            createPabelContainer.Remove(type);
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 获取预制体附带信息
    /// </summary>
    protected abstract UIPrefabInfo GetPanelInfo(string prefabName);
}
