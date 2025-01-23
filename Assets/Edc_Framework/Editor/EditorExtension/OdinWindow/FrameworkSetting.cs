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
        tree.AddAssetAtPath("对象池管理", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/ObjectPool/ObjectPoolSetting.asset");
        tree.AddAllAssetsAtPath("UI面板设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI",typeof(ViewPrefabSetting));
        tree.AddAllAssetsAtPath("UI面板设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI",typeof(PersistentViewPrefabSetting));
        tree.AddAllAssetsAtPath("UI面板设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/UI",typeof(WindowPrefabSetting));
        tree.AddAssetAtPath("输入键位设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Input/InputSetting.asset");
        tree.AddAssetAtPath("本地化字体设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Localization/LocalizationFontSetting.asset");
        tree.AddAssetAtPath("红点树设置","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/RedDotTree/RedDotTreeSetting.asset");
        tree.AddAssetAtPath("打包设置工具", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Build/BuildSettingTool.asset");
        tree.AddAssetAtPath("AB包打包设置", "Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Build/AssetBundlesTool.asset");
        return tree;
    }
}