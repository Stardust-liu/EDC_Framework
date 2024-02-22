using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

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
        tree.AddAssetAtPath("对象池管理", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/ObjectPoolSetting.asset");
        tree.AddAssetAtPath("视图界面UI", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/ViewSetting.asset");
        tree.AddAssetAtPath("窗口UI", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/WindowSetting.asset");
        tree.AddAssetAtPath("输入键位设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/InputSetting.asset");
        tree.AddAllAssetsAtPath("音频管理","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting", typeof(SoundBgSetting));
        tree.AddAllAssetsAtPath("音频管理","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting", typeof(SoundEffectSetting));
        tree.AddAllAssetsAtPath("音频管理","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting", typeof(SoundDialogueSetting));
        return tree;
    }
}