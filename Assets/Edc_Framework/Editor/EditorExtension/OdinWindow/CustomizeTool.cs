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
        tree.AddAssetAtPath("替换字体", "Assets/Edc_Framework/Sources/AssetFile/CustomizeTool/ChangeFontTool.asset");
        tree.AddAssetAtPath("替换材质球", "Assets/Edc_Framework/Sources/AssetFile/CustomizeTool/ChangeMaterialTool.asset");
        tree.AddAllAssetsAtPath("图集设置", "Assets/Edc_Framework/Sources/AssetFile/CustomizeTool/SpriteAtlasTool",typeof(CreateAtlasSetting));
        tree.AddAllAssetsAtPath("图集设置", "Assets/Edc_Framework/Sources/AssetFile/CustomizeTool/SpriteAtlasTool",typeof(AtlasManager));
        //tree.AddAllAssetsAtPath("图集设置", "Assets/Edc_Framework/Sources/AssetFile/CustomizeTool/SpriteAtlasTool",typeof(RepeatSpriteCheck));
        return tree;
    }
}