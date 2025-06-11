using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ArchiveTool", menuName = "创建.Assets文件/FrameworkTool/ArchiveTool")]
public class ArchiveTool : SerializedScriptableObject
{
    [Button("打开存档文件夹", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    public void OpenArchiveFile(){
        Process.Start(Application.persistentDataPath);
    }

    [PropertySpace(10)]
    [Button("清空存档", ButtonSizes.Medium), GUIColor(1f, 0.5f, 0.5f)]
    public void ClearArchiveFile(){
        if(EditorUtility.DisplayDialog("注意！", "确认移除项目存档吗", "确认","取消")){
            Directory.Delete(Application.persistentDataPath, true);
        }
    }
}
