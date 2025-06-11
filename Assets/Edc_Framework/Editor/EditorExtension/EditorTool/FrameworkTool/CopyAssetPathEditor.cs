using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CopyAssetPathEditor
{
     // 添加右键菜单项
    [MenuItem("Assets/复制Asset路径", false)]
    private static void CopyAssetPath()
    {
        // 获取选中的资源
        var selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogWarning("未选择资产");
            return;
        }

        // 获取资源的路径
        string path = AssetDatabase.GetAssetPath(selected);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("选定的对象不是资产");
            return;
        }

        // 将路径复制到剪贴板
        GUIUtility.systemCopyBuffer = path;
        Debug.Log($"已将资产路径复制到剪贴板: {path}");
    }
}
