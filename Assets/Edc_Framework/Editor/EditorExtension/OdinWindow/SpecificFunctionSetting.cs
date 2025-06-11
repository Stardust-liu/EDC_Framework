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
        return tree;
    }
}