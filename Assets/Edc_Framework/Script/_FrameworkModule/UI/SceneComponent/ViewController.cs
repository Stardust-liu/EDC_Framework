using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : BaseMonoIOCComponent
{
    public RectTransform inactiveView_3DUI;
    public RectTransform activeView_3DUI;
    public RectTransform inactiveView_UI;
    public RectTransform activeView_UI;
    private static BaseView_P currentView;
    private static HashSet<BaseView_P> openViewInfo;
    private static Stack<BaseView_P> openViewStack;
    private static Dictionary<string, BaseView_P> viewInstanceDictionary;
    public static ViewPrefabSetting View{get; private set;}

    protected override void Init(){
        View = Hub.Resources.GetScriptableobject<ViewPrefabSetting>(nameof(ViewPrefabSetting));
        openViewInfo = new HashSet<BaseView_P>();
        openViewStack = new Stack<BaseView_P>();
        viewInstanceDictionary = new Dictionary<string, BaseView_P>();
    }

    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView(BaseView_P baseView_P, bool isBackLastView = false){
        if(!isBackLastView){
            UpdateViewStatic(baseView_P);
        }
        if(baseView_P == currentView){
            LogManager.LogWarning($"切换的view {baseView_P.GetType().Name} 为当前正在显示的view");
        }
        if(!baseView_P.IsCreate){
            baseView_P.CreateUiPrefab();
        }
        if(currentView != null){
            ((IBaseView_P)currentView).Hide(OpenView, baseView_P);
        }
        else{
            OpenView(baseView_P);
        }
        baseView_P.PreShowEffect();
    }

    /// <summary>
    /// 关闭视图
    /// </summary>
    public void CloseView(){
        ((IBaseView_P)currentView)?.Hide();
        currentView = null;
    }

    /// <summary>
    /// 返回上一个视图页面
    /// </summary>
    public void BackLastView(){
        if(openViewStack.Count > 1){
            openViewInfo.Remove(openViewStack.Pop());
            ChangeView(openViewStack.Peek(), false);
        }
        else{
            LogManager.LogError("没有上一个页面");
        }
    }

    /// <summary>
    /// 设置View单例
    /// </summary>
    /// 在通过场景名获得默认View时会这样做
    public static void SetView(string classNmame, BaseView_P instance){
        if(!viewInstanceDictionary.ContainsKey(classNmame)){
            viewInstanceDictionary.Add(classNmame, instance);
        }
    }

    /// <summary>
    /// 获取View单例
    /// </summary>
    public static T GetView<T>(string classNmame) where T: BaseView_P{
        if(viewInstanceDictionary.ContainsKey(classNmame)){
            return (T)viewInstanceDictionary[classNmame];
        }
        else{
            var instance = Activator.CreateInstance<T>();
            viewInstanceDictionary.Add(classNmame, instance);
            return instance;
        }
    }

    /// <summary>
    /// 销毁视图
    /// </summary>
    public void DestroyView(string className){
        if(viewInstanceDictionary.ContainsKey(className)){
            viewInstanceDictionary.Remove(className);
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    private void OpenView(BaseView_P _currentView){
        currentView = _currentView;
        if(!currentView.IsCreate){
            currentView.CreateUiPrefab();
        }
        ((IBaseUI_P)currentView).Show();
    }

    private void UpdateViewStatic(BaseView_P baseView_P){
        if(openViewInfo.Contains(baseView_P)){
            while(openViewStack.Peek() != baseView_P){
                openViewInfo.Remove(openViewStack.Pop());
            }
        }
        else{
            openViewStack.Push(baseView_P);
            openViewInfo.Add(baseView_P);
        }
    }
}
