using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewManager
{
    public static Transform parent;
    public static Transform persistentViewParent;
    private static BaseView_C currentView;
    private static Dictionary<string, BaseView_C> viewInstanceDictionary;
    private static List<BaseView_C> persistentViewList;
    private static float showNextViewWaitTime;
    private static TransitionsType transitionsType;

    public static Transform Parent{get; private set;}
    public static ViewPrefabSetting View{get; private set;}

    public ViewManager(Transform parent, Transform persistentViewParent, ViewPrefabSetting view){
        ViewManager.persistentViewParent = persistentViewParent;
        Parent = parent;
        View = view;
        viewInstanceDictionary = new Dictionary<string, BaseView_C>();
        persistentViewList = new List<BaseView_C>();
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
    public void ChangeView(BaseView_C currentView){
        if(currentView == ViewManager.currentView){
            LogManager.LogWarning($"切换的view {currentView.GetType().Name} 为当前正在显示的view");
        }
        if(!currentView.IsCreate){
            currentView.SetPrefabInfo();
        }
        if(ViewManager.currentView == null){
            ViewManager.currentView = currentView;
            ApplyShowIfRequired();
        }
        else{
            ViewManager.currentView.Hide();
            showNextViewWaitTime = ViewManager.currentView.ShowNextViewWaitTime;
            if(CheckGradientEffectConflict(currentView)){
                LogManager.LogWarning($"View界面切换渐变效果冲突，放弃 {transitionsType.GetType().Name} 的淡入效果");
                transitionsType = currentView.TransitionsType;
                ApplyFadeOut(currentView.FadeOutColor);
            }
            else{
                if(!ApplyFadeOutIfRequired(ViewManager.currentView.FadeOutColor)){//检测当前View是否需要淡出
                    transitionsType = currentView.TransitionsType;
                    if(ApplyFadeInIfRequired(currentView.FadeInColor)){//-----------检测下一个View是否需要淡入
                        ViewManager.currentView.StartWaitHideFinish();
                        //如果下一个界面要用到淡入效果，则当前界面也会使用淡出效果（使用下一个View的淡入颜色）
                        //因此需要调用等待关闭完成方法
                    }
                }
            }
            ViewManager.currentView = currentView; 
            if(showNextViewWaitTime == 0){
                OpenView();
            }
            else{
                FrameworkManager.instance.StartCoroutine(WaitOpenView());
            }
        }
    }

    /// <summary>
    /// 设置ViewPresenter单例
    /// </summary>
    /// 在通过场景名获得默认View时会这样做
    public static void SetView(string classNmame, BaseView_C instance){
        if(!viewInstanceDictionary.ContainsKey(classNmame)){
            viewInstanceDictionary.Add(classNmame, instance);
        }
    }

    /// <summary>
    /// 获取ViewPresenter单例
    /// </summary>
    public static T GetView<T>(string classNmame) where T: BaseView_C{
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
            if(viewInstanceDictionary[className].IsCreate){
                viewInstanceDictionary[className].Destroy();
            }
            else{
                viewInstanceDictionary.Remove(className);
            }
        }
    }

    /// <summary>
    /// 打开常驻视图
    /// </summary>
    public void OpenPersistentView(BaseView_C persistentView){
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
    public void HidePersistentView(BaseView_C persistentView){
        persistentView.Hide();
    }

    /// <summary>
    /// 销毁常驻视图
    /// </summary>
    public void DestroyPersistentView(BaseView_C persistentView){
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



    private IEnumerator WaitOpenView(){
        Hub.EventCenter.OnEvent(EventName.enterRestriction);
        yield return new WaitForSeconds(showNextViewWaitTime);
        Hub.EventCenter.OnEvent(EventName.exitRestriction);
        OpenView();
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

    /// <summary>
    /// 检测是否需要直接显示淡入颜色
    /// </summary>
    private void ApplyShowIfRequired(){
        transitionsType = currentView.TransitionsType;
        if(transitionsType == TransitionsType.FadeIn||
            transitionsType == TransitionsType.FadeInFadeOut){
            Hub.LoadPanel.Show(currentView.FadeInColor);
        }          
    }
    
    /// <summary>
    /// 检测是否需要淡出效果
    /// </summary>
    private bool ApplyFadeOutIfRequired(Color fadeOutColor){
        if(transitionsType == TransitionsType.FadeOut||
            transitionsType == TransitionsType.FadeInFadeOut){
            showNextViewWaitTime = WaitTime.viewGradientTime;
            Hub.LoadPanel.FadeIn(fadeOutColor);
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 应用淡出效果
    /// </summary>
    private void ApplyFadeOut(Color fadeOutColor){
        showNextViewWaitTime = WaitTime.viewGradientTime;
        Hub.LoadPanel.FadeIn(fadeOutColor);
    }
    
    /// <summary>
    /// 检测是否需要淡入效果
    /// </summary>
    private bool ApplyFadeInIfRequired(Color FadeIn){
        if(transitionsType == TransitionsType.FadeIn||
            transitionsType == TransitionsType.FadeInFadeOut){
            showNextViewWaitTime = WaitTime.viewGradientTime;
            Hub.LoadPanel.FadeIn(FadeIn);
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 检查渐变效果是否冲突
    /// </summary>
    private bool CheckGradientEffectConflict(BaseView_C currentView){
        //上一个UI界面是否有淡出效果
        bool lastViewIsFadeOut = transitionsType == TransitionsType.FadeOut || transitionsType == TransitionsType.FadeInFadeOut;
        
        //当前UI界面是否有淡入效果
        bool currentViewIsFadeIn = currentView.TransitionsType == TransitionsType.FadeIn || currentView.TransitionsType == TransitionsType.FadeInFadeOut;

        return lastViewIsFadeOut && currentViewIsFadeIn;
    }
}
