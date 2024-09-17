using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ViewSetting", menuName = "创建.Assets文件/ViewSetting")]
public class ViewPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, UIPrefabInfo> viewPrefab;
    public Dictionary<string, BaseView_P> sceneDefaultView;


    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public UIPrefabInfo GetPrefabInfo(string name){
        return viewPrefab[name];
    }


    /// <summary>
    /// 获取场景默认视图
    /// </summary>
    public BaseView_P GetSceneDefaultView(string sceneNmae){
        if(sceneDefaultView.ContainsKey(name)){
            var instance = sceneDefaultView[name];
            ViewController.SetView(instance.GetType().Name, instance);
            return instance;
        }
        else{
            LogManager.LogWarning($"场景：{name}没有配置默认View");
            return null;
        }
    }
}
