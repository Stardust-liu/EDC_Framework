using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class ResourceProcessingTool : OdinMenuEditorWindow
{
    [MenuItem("Customize/ResourceProcessingTool")]
    private static void OpenWindow()
    {
        GetWindow<ResourceProcessingTool>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        tree.AddAssetAtPath("图像设置", "Assets/Edc_Framework/Sources/AssetFile/ResourceProcessingTool/TextureSettingTool.asset");
        tree.AddAssetAtPath("音频设置", "Assets/Edc_Framework/Sources/AssetFile/ResourceProcessingTool/AudioSettingTool.asset");
        tree.AddAssetAtPath("模型设置", "Assets/Edc_Framework/Sources/AssetFile/ResourceProcessingTool/ModelSettingTool.asset");
        return tree;
    }
}
