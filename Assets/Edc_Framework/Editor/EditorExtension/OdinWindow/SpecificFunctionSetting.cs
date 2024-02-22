using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class SpecificFunctionSetting : OdinMenuEditorWindow
{
    [MenuItem("Customize/SpecificFunctionSetting")]
    private static void OpenWindow()
    {
        GetWindow<SpecificFunctionSetting>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.AddAssetAtPath("角色信息及文本设置","Assets/Edc_Framework/Sources/Resources/AssetFile/CharacterTextSetting.asset");
        return tree;
    }
}