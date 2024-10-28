using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentViewController : MonoBehaviour
{
    public RectTransform inactivePersistentView_3DUI;
    public RectTransform activePersistentView_3DUI;
    public RectTransform inactivePersistentView_UI;
    public RectTransform activePersistentView_UI;
    private static List<BasePersistentView_P> persistentViewList;
    public static Dictionary<string, BasePersistentView_P> viewInstanceDictionary;
    public static PersistentViewPrefabSetting PersistentView{get; private set;}


    public void Init(){
        PersistentView = Hub.Resources.GetScriptableobject<PersistentViewPrefabSetting>(nameof(PersistentViewPrefabSetting));
        persistentViewList = new List<BasePersistentView_P>();
        viewInstanceDictionary = new Dictionary<string, BasePersistentView_P>();
    }

    /// <summary>
    /// 打开常驻视图
    /// </summary>
    public void OpenPersistentView(BasePersistentView_P persistentView){
        if(!persistentView.IsCreate){
            persistentView.CreateUiPrefab();
        }
        if(!persistentView.IsShow){
           ((IBaseUI_P)persistentView).Show();
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
    public void HidePersistentView(BasePersistentView_P persistentView){
        ((IBasePersistentView_P)persistentView).Hide();
    }

    /// <summary>
    /// 销毁常驻视图
    /// </summary>
    public void DestroyPersistentView(BasePersistentView_P persistentView){
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
                ((IBasePersistentView_P)item).Hide();
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
    /// 获取View单例
    /// </summary>
    public static T GetView<T>(string classNmame) where T: BasePersistentView_P{
        if(viewInstanceDictionary.ContainsKey(classNmame)){
            return (T)viewInstanceDictionary[classNmame];
        }
        else{
            var instance = Activator.CreateInstance<T>();
            viewInstanceDictionary.Add(classNmame, instance);
            return instance;
        }
    }
}
