using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public Image windowDarkBG_3DUI; 
    public Image windowDarkBG_UI; 
    public RectTransform inactiveWindow_3DUI; 
    public RectTransform activeWindow_3DUI; 
    public RectTransform inactiveWindow_UI;
    public RectTransform activeWindow_UI;
    public static Dictionary<string, BaseWindow_P> baseWindowDictionary;
    public static Stack<BaseWindow_P> openWindowStack;
    public static BaseWindow_P currentWindow;
    public static WindowPrefabSetting Window {get; private set;}

    public void Init(){
        Window = Hub.Resources.GetScriptableobject<WindowPrefabSetting>(nameof(WindowPrefabSetting));
        baseWindowDictionary = new Dictionary<string, BaseWindow_P>();
        openWindowStack = new Stack<BaseWindow_P>();
    }

    /// <summary>
    /// 打开窗口界面
    /// </summary>
    public void OpenWindow(BaseWindow_P baseWindow_P){
        if(baseWindow_P == currentWindow){
            LogManager.LogWarning($"打开的window {baseWindow_P.WindowName} 为当前正在显示的window");
        }
        currentWindow?.DownLaye();
        openWindowStack.Push(baseWindow_P);
        currentWindow = baseWindow_P;
        OpenWindow();
        if(currentWindow.is3DUI){
            windowDarkBG_3DUI.enabled = true;
        }
        else{
            windowDarkBG_UI.enabled = true;
        }
    }
    
    /// <summary>
    /// 关闭窗口界面
    /// </summary>
    public void CloseWindow(){
        CloseWindow(null);
    }

    /// <summary>
    /// 关闭窗口界面
    /// </summary>
    public void CloseWindow(Action hideFinishCallBack = null){
        ((IBaseWindow_P)currentWindow).Hide(hideFinishCallBack);
        openWindowStack.Pop();
        if(openWindowStack.Count != 0){
            currentWindow = openWindowStack.Peek();
            currentWindow.SetToTopLayer();
        }
        else{
            windowDarkBG_3DUI.enabled = false;
            windowDarkBG_UI.enabled = false;
            currentWindow = null;
        }
    }

    /// <summary>
    /// 获取Window单例
    /// </summary>
    public static T GetWindow<T>(string classNmame) where T: BaseWindow_P{
        if(baseWindowDictionary.ContainsKey(classNmame)){
            return (T)baseWindowDictionary[classNmame];
        }
        else{
            var instance = Activator.CreateInstance<T>();
            baseWindowDictionary.Add(classNmame, instance);
            return instance;
        }
    }

    /// <summary>
    /// 销毁窗口
    /// </summary>
    public void DestroyWindow(string className){
        if(baseWindowDictionary.ContainsKey(className)){
            baseWindowDictionary.Remove(className);
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    private void OpenWindow(){
        if(!currentWindow.IsCreate){
            currentWindow.CreateUiPrefab();
        }
        ((IBaseUI_P)currentWindow).Show();
    }
}
