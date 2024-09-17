using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public RectTransform inactiveView;
    public RectTransform activeView;
    public TweenFade tweenFade;
    private static BaseView_P currentView;
    private static Dictionary<string, BaseView_P> viewInstanceDictionary;
    public static ViewPrefabSetting View{get; private set;}

    public void Init(){
        View = Hub.Framework.view;
        viewInstanceDictionary = new Dictionary<string, BaseView_P>();
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
    public void ChangeView(BaseView_P baseView_P){
        if(baseView_P == currentView){
            LogManager.LogWarning($"切换的view {baseView_P.GetType().Name} 为当前正在显示的view");
        }
        if(!baseView_P.IsCreate){
            baseView_P.CreateUiPrefab();
        }
        ((IBaseView_P)currentView)?.Hide(OpenView, baseView_P);
    }

    /// <summary>
    /// 关闭视图
    /// </summary>
    public void CloseView(){
        ((IBaseView_P)currentView)?.Hide();
        currentView = null;
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
}
