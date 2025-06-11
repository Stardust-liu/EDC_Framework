using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class FrameworkSetting : OdinMenuEditorWindow
{   
    [MenuItem("Customize/FrameworkSetting")]
    private static void OpenWindow()
    {
        GetWindow<FrameworkSetting>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Add("常用颜色管理", CommonColorsSetting.Instance);
        tree.AddAssetAtPath("对象池管理", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/ObjectPool/ObjectPoolSetting.asset");
        tree.AddAllAssetsAtPath("UI面板设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI",typeof(ViewSetting));
        tree.AddAllAssetsAtPath("UI面板设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI",typeof(PersistentViewSetting));
        tree.AddAllAssetsAtPath("UI面板设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI",typeof(WindowSetting));
        tree.AddAssetAtPath("输入键位设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Input/InputSetting.asset");
        tree.AddAssetAtPath("本地化设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Localization/LocalizationSetting.asset");
        tree.AddAssetAtPath("红点树设置","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/RedDotTree/RedDotTreeSetting.asset");
        tree.AddAssetAtPath("打包设置工具", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Build/BuildSettingTool.asset");
        tree.AddAssetAtPath("AB包打包设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Build/AssetBundlesTool.asset");
        tree.AddAssetAtPath("存档信息设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Archive/ArchiveTool.asset");
        return tree;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLocalizationSettingInfo();
        SceneView.duringSceneGui += OnSceneGUI;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SceneView.duringSceneGui -= OnSceneGUI;
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingEditMode){
         
            //Debug.Log("初始化完成");
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        ShwoLocalizationSetting();
        Handles.EndGUI();
    }

    public LocalizationSetting localizationSetting;
    public SystemLanguage selectedLanguage = SystemLanguage.ChineseSimplified;

    /// <summary>
    /// 设置多语言相关设置
    /// </summary>
    private void SetLocalizationSettingInfo(){
        var path = "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Localization/LocalizationSetting.asset";
        localizationSetting = AssetDatabase.LoadAssetAtPath<LocalizationSetting>(path);
    }

    /// <summary>
    /// 显示多语言相关设置
    /// </summary>
    private void ShwoLocalizationSetting(){
        GUILayout.BeginArea(new Rect(40, 2,  Screen.width, Screen.height));
        var languageSupport = localizationSetting.LanguageSupport;

        if(EditorGUILayout.DropdownButton(new GUIContent(selectedLanguage.ToString()), FocusType.Keyboard, GUILayout.Width(150))){
            GenericMenu menu = new GenericMenu();
            foreach (var item in languageSupport)
            {
                var key = item.Key;
                menu.AddItem(new GUIContent(key.ToString()), selectedLanguage == key, ()=> 
                {
                    selectedLanguage = key;
                    ChangeLange();
                });
            }
            menu.DropDown(new Rect(new Vector2(0,22), Vector2.zero));
        }
        GUILayout.EndArea();
    }

    /// <summary>
    /// 修改语言
    /// </summary>
    private void ChangeLange(){
        if(!Application.isPlaying){
            var comp = GameObject.FindObjectsOfType<LocalizationGroup>(true);
            foreach (var item in comp)
            {
                ((IEditorChangeLange)item).ChangeLanguage(selectedLanguage);
            }
        }
        else{
            if(FrameworkManager.isInitFinish){
                Hub.Localization.ChangeLange(selectedLanguage);
            }
            else{
                Debug.LogWarning("等待框架初始化完成再切换语言");
            }
        }
    }
}