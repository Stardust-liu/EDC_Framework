using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class CustomizeTool : OdinMenuEditorWindow
{
    [MenuItem("Customize/EditorTool")]
    private static void OpenWindow()
    {
        GetWindow<CustomizeTool>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.AddAssetAtPath("替换字体", "Assets/Edc_Framework/Sources/AssetFile/EditorTool/ChangeFontTool.asset");
        tree.AddAllAssetsAtPath("资源设置工具", "Assets/Edc_Framework/Sources/AssetFile/EditorTool/OptimizationTool", typeof(TextureSettingTool));
        tree.AddAllAssetsAtPath("资源设置工具", "Assets/Edc_Framework/Sources/AssetFile/EditorTool/OptimizationTool", typeof(AudioSettingTool));
        tree.AddAllAssetsAtPath("资源设置工具", "Assets/Edc_Framework/Sources/AssetFile/EditorTool/OptimizationTool", typeof(ModelSettingTool));
        tree.AddAssetAtPath("打包设置工具", "Assets/Edc_Framework/Sources/AssetFile/EditorTool/BuildSettingTool.asset");
        tree.AddAssetAtPath("AB包打包设置", "Assets/Edc_Framework/Sources/AssetFile/EditorTool/AssetBundlesTool.asset");
        return tree;
    }
}