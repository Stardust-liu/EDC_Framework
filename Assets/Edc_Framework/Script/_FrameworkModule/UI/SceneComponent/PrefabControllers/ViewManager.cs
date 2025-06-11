using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : PanelManager
{
    private BaseView currentView;
    private HashSet<BaseView> openViewInfo;
    private Stack<BaseView> openViewStack;
    public ViewSetting ViewSetting{get; private set;}

    protected override void Init(){
        base.Init();
        var resourcePath = new ResourcePath("ViewSetting","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI/ViewSetting.asset");
        ViewSetting = Hub.Resources.GetScriptableobject<ViewSetting>(resourcePath);
        openViewInfo = new HashSet<BaseView>();
        openViewStack = new Stack<BaseView>();
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView<T>(Action hideFinishCallBack = null) where T : BaseView
    {
        if(currentView != null){
            var closePanelType = currentView.GetType();
            ClosePanel(closePanelType, hideFinishCallBack += OpenView<T>);
        }
        else{
            OpenView<T>();
        }
        
        if(openViewInfo.Contains(currentView)){
            while(openViewStack.Peek() != currentView){
                openViewInfo.Remove(openViewStack.Pop());
            }
        }
        else{
            openViewStack.Push(currentView);
            openViewInfo.Add(currentView);
        }
    }

    /// <summary>
    /// 返回上一个视图页面
    /// </summary>
    public void BackLastView(Action hideFinishCallBack = null){
        if(openViewStack.Count > 1){
            openViewInfo.Remove(openViewStack.Pop());
            var closePanelType = currentView.GetType();
            currentView = openViewStack.Peek();
            ClosePanel(closePanelType, hideFinishCallBack += WaitOpenView);
        }
        else{
            LogManager.LogError("没有上一个页面");
        }
    }

    private void OpenView<T>()where T : BaseView
    {
        OpenPanel<T>();
        currentView = GetPanel<T>();
    }
    private void WaitOpenView()
    {
        OpenPanel(currentView.GetType());
    }

    protected override UIPrefabInfo GetPanelInfo(string prefabName)
    {
        return ViewSetting.GetPanelInfo(prefabName);
    }
}
