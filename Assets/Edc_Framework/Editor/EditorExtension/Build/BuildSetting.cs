using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class BuildSetting : OdinMenuEditorWindow
{
    [MenuItem("Customize/BuildSetting")]
    private static void OpenWindow()
    {
        GetWindow<BuildSetting>("打包设置").Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.AddAssetAtPath("项目基础信息", BuildProjectInfo.AssetPath);
        tree.Add("打包前框架检查", new EdcFrameworkCheckerTool());
        tree.AddAssetAtPath("打包设置工具", BuildSettingTool.AssetPath);
        return tree;
    }
}
