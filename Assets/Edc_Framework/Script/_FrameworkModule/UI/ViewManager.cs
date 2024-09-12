using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewManager
{
    private static BaseView_P currentView;
    private static Dictionary<string, BaseView_P> viewInstanceDictionary;
    private static List<BaseView_P> persistentViewList;
    public static RectTransform PersistentViewParent{get; private set;}
    public static RectTransform Parent{get; private set;}
    public static ViewPrefabSetting View{get; private set;}

    public ViewManager(){
        var framework = Hub.Framework;
        PersistentViewParent = framework.persistentViewLayer;
        Parent = framework.viewLayer;
        View = framework.view;
        viewInstanceDictionary = new Dictionary<string, BaseView_P>();
        persistentViewList = new List<BaseView_P>();
    }

    /// <summary>
    /// 切换至场景默认视图
    /// </summary>
    public void ChangeSceneDefaultView(string sceneName){
        ChangeView(View.GetSceneDefaultView(sceneName));
    }


    /// <summary>
    /// 切换视图界面
    /// </summary>
    public void ChangeView(BaseView_P currentView){
        if(currentView == ViewManager.currentView){
            LogManager.LogWarning($"切换的view {currentView.GetType().Name} 为当前正在显示的view");
        }
        if(!currentView.IsCreate){
            currentView.SetPrefabInfo();
        }
        if(ViewManager.currentView != null){
            ViewManager.currentView.Hide();
        }
        ViewManager.currentView = currentView;
        OpenView();
    }

    /// <summary>
    /// 设置ViewPresenter单例
    /// </summary>
    /// 在通过场景名获得默认View时会这样做
    public static void SetView(string classNmame, BaseView_P instance){
        if(!viewInstanceDictionary.ContainsKey(classNmame)){
            viewInstanceDictionary.Add(classNmame, instance);
        }
    }

    /// <summary>
    /// 获取ViewPresenter单例
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
    /// 打开常驻视图
    /// </summary>
    public void OpenPersistentView(BaseView_P persistentView){
        if(!persistentView.IsCreate){
            persistentView.SetPrefabInfo();
            persistentView.CreateUiPrefab();
        }
        if(!persistentView.IsShow){
            persistentView.Show();
        }
        else{
            LogManager.LogWarning($"打开了一个正在显示中的常驻视图 {persistentView.GetType().Name}");
        }
        if(!persistentViewList.Contains(persistentView)){
            persistentViewList.Add(persistentView);
        }
    }

    /// <summary>
    /// 隐藏常驻视图
    /// </summary>
    public void HidePersistentView(BaseView_P persistentView){
        persistentView.Hide();
    }

    /// <summary>
    /// 销毁常驻视图
    /// </summary>
    public void DestroyPersistentView(BaseView_P persistentView){
        persistentView.Destroy();
        persistentViewList.Remove(persistentView);
    }

    private void DestroyPersistentView(int index){
        persistentViewList[index].Destroy();
        persistentViewList.RemoveAt(index);
    }

    /// <summary>
    /// 隐藏所有常驻视图
    /// </summary>
    public void HideAllPersistentView(){
        foreach (var item in persistentViewList)
        {
            if(item.IsShow){
                item.Hide();
            }
        }
    }

    /// <summary>
    /// 销毁所有常驻视图
    /// </summary>
    public void DestroyAllPersistentView(){
        for (int i = persistentViewList.Count - 1; i >= 0; i--)
        {
            DestroyPersistentView(i);
        }
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    private void OpenView(){
        if(!currentView.IsCreate){
            currentView.CreateUiPrefab();
        }
        currentView.Show();
        Hub.LoadPanel.FadeOut();
    }
}
