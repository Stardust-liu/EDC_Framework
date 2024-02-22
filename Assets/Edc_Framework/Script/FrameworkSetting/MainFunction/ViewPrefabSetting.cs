using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public enum TransitionsType{
    None,           //无
    FadeIn,         //进入时有渐变效果
    FadeOut,        //退出时有渐变效果
    FadeInFadeOut,  //都有渐变效果
}

public class BseViewPrefabInfo{
    public GameObject prefab;
    [LabelText("在关闭完成后销毁对象")]
    public bool isHideFinishDestroy;
}

public class ViewPrefabInfo{
    public GameObject prefab;
    [LabelText("在关闭完成后销毁对象")]
    public bool isHideFinishDestroy;
    [LabelText("下个界面延迟打开时间")]
    [ShowIf("ShowNextViewWaitTimeShowIf")]
    public float showNextViewWaitTime;
    [LabelText("淡入淡出类型")]
    public TransitionsType transitionsType;

    [LabelText("淡入颜色")]
    [ShowIf("FadeInColorShowIf")]
    public Color fadeInColor;

    [LabelText("淡出颜色")]
    [ShowIf("FadeOutColorShowIf")]
    public Color fadeOutColor;

    private bool FadeInColorShowIf(){
        if(transitionsType == TransitionsType.FadeIn||
           transitionsType == TransitionsType.FadeInFadeOut){
            return true;
        }
        else{
            return false;
        }
    }

    private bool FadeOutColorShowIf(){
        if(transitionsType == TransitionsType.FadeOut||
           transitionsType == TransitionsType.FadeInFadeOut){
            return true;
        }
        else{
            return false;
        }
    }


    private bool ShowNextViewWaitTimeShowIf(){
        if(transitionsType == TransitionsType.None||
           transitionsType == TransitionsType.FadeIn){
            return true;
        }
        else{
            return false;
        }
    }
}

[CreateAssetMenu(fileName = "ViewPrefabSetting", menuName = "创建Assets文件/ViewPrefabSetting")]
public class ViewPrefabSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, ViewPrefabInfo> uiPrefab;

    [DictionaryDrawerSettings(KeyLabel = "UI名字", ValueLabel ="UI对象信息")]
    public Dictionary<string, BseViewPrefabInfo> persistentUiPrefab;
    
    public Dictionary<string, BaseView_C> sceneDefaultView;


    /// <summary>
    /// 获取对应的UI预制体信息
    /// </summary>
    public ViewPrefabInfo GetPrefabInfo(string name){
        return uiPrefab[name];
    }

    /// <summary>
    /// 获取对应的常驻UI信息
    /// </summary>
    public BseViewPrefabInfo GetPersistentPrefabInfo(string name){
        return persistentUiPrefab[name];
    }

    /// <summary>
    /// 获取场景默认视图
    /// </summary>
    public BaseView_C GetSceneDefaultView(string sceneNmae){
        if(sceneDefaultView.ContainsKey(name)){
            var instance = sceneDefaultView[name];
            ViewManager.SetView(instance.GetType().Name, instance);
            return instance;
        }
        else{
            LogManager.LogWarning($"场景：{name}没有配置默认View");
            return null;
        }
    }
}
