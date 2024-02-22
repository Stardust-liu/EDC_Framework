using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager
{
    private static Image windowDarkBG; 
    private static RectTransform darkBGTranform;
    public static Dictionary<string, BaseWindow_C> baseWindowDictionary;
    public static Stack<BaseWindow_C> openWindowStack;
    public static BaseWindow_C currentWindow;
    public static Transform Parent {get; private set;}
    public static WindowPrefabSetting Window {get; private set;}

    public WindowManager(Transform parent, Image windowDarkBG, WindowPrefabSetting window){
        WindowManager.windowDarkBG = windowDarkBG;
        Parent = parent;
        Window = window;
        darkBGTranform = windowDarkBG.rectTransform;
        baseWindowDictionary = new Dictionary<string, BaseWindow_C>();
        openWindowStack = new Stack<BaseWindow_C>();
    }

    /// <summary>
    /// 打开窗口界面
    /// </summary>
    public void OpenWindow(BaseWindow_C baseWindow_P){
        if(baseWindow_P == WindowManager.currentWindow){
            LogManager.LogWarning($"打开的window {baseWindow_P.GetType().Name} 为当前正在显示的window");
        }

        darkBGTranform.SetSiblingIndex(openWindowStack.Count);
        windowDarkBG.enabled = true;
        currentWindow = baseWindow_P;
        openWindowStack.Push(baseWindow_P);
        OpenWindow();
    }

    /// <summary>
    /// 关闭窗口界面
    /// </summary>
    public void CloseWindow(){
        darkBGTranform.SetSiblingIndex(openWindowStack.Count-2);
        currentWindow.Hide();
        openWindowStack.Pop();
        if(openWindowStack.Count != 0){
            currentWindow = openWindowStack.Peek();
        }
        else{
            windowDarkBG.enabled = false;
            currentWindow = null;
        }
    }

    /// <summary>
    /// 获取ViewPresenter单例
    /// </summary>
    public static T GetWindow<T>(string classNmame) where T: BaseWindow_C{
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
            if(baseWindowDictionary[className].IsCreate){
                baseWindowDictionary[className].Destroy();
            }
            else{
                baseWindowDictionary.Remove(className);
            }
        }
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    private void OpenWindow(){
        if(!currentWindow.IsCreate){
            currentWindow.CreateUiPrefab();
        }
        currentWindow.Show();
    }
}
