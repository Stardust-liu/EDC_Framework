using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using AssetBundleBrowser;

[CreateAssetMenu(fileName = "AssetBundlesTool", menuName = "创建.Assets文件/FrameworkTool/AssetBundlesTool")]
public class AssetBundlesTool : SerializedScriptableObject
{
    [Button("打开AssetBundle Broswer", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    public void OpenAssetBundleBroswer(){

        AssetBundleBrowserMain window = (AssetBundleBrowserMain)EditorWindow.GetWindow(typeof(AssetBundleBrowserMain));
        window.Show();
    }
}
