using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    Text,
    TextMeshPro,
    TextMeshProUGUI,
}

[CreateAssetMenu(fileName = "ChangeFontTool", menuName = "创建.Assets文件/CustomizeTool/ChangeFontTool")]
public class ChangeFontTool : SerializedScriptableObject
{
    [FoldoutGroup("按照GameObject查找")]
    public List<GameObject> prefabList;
    [FoldoutGroup("按照GameObject查找")]
    [HideIf("textType", TextType.Text)]
    public TMP_FontAsset targetFont_Plugin;
    [FoldoutGroup("按照GameObject查找")]
    [ShowIf("textType", TextType.Text)]
    public Font targetfont_Native;
    [FoldoutGroup("按照GameObject查找")]
    public Material material;
    [FoldoutGroup("按照GameObject查找")]
    public TextType textType;    

    [FoldoutGroup("按照GameObject查找")]
    [OnInspectorGUI]
    private void ChangeFontToolBtn_Obj()
    {
        using (new GUILayout.HorizontalScope())
        {
            GUI.color = new Color(0.5f, 0.8f, 1);
            if (GUILayout.Button("修改字体", GUILayout.Height(30)))
            {
                ChangeFont_Obj();
            }

            GUI.color = new Color(1, 0.5f, 0.5f);
            if (GUILayout.Button("清空列表", GUILayout.Height(30)))
            {
                ClickCleanPrefabList_Obj();
            }
            GUI.color = Color.white;
        }
    }

    public void ChangeFont_Obj()
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            EditorUtility.SetDirty(prefabList[i]);
            switch (textType)
            {
                case TextType.Text:
                    if (targetfont_Native == null)
                        break;
                    var textList = prefabList[i].GetComponentsInChildren<Text>(true);
                    foreach (var item in textList)
                    {
                        item.font = targetfont_Native;
                        item.material  = material;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(item);
                    }
                    break;
                case TextType.TextMeshPro:
                    if (targetFont_Plugin == null)
                        break;
                    var textmeshProList = prefabList[i].GetComponentsInChildren<TextMeshPro>(true);
                    foreach (var item in textmeshProList)
                    {
                        item.font = targetFont_Plugin;
                        item.material  = material;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(item);
                    }
                    break;
                case TextType.TextMeshProUGUI:
                    if (targetFont_Plugin == null)
                        break;
                    var textMeshProUGUIList = prefabList[i].GetComponentsInChildren<TextMeshProUGUI>(true);
                    foreach (var item in textMeshProUGUIList)
                    {
                        item.font = targetFont_Plugin;
                        item.material  = material;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(item);
                    }
                    break;
            }
        }
        Debug.Log("修改完成");
    }

    private void ClickCleanPrefabList_Obj()
    {
        prefabList.Clear();
    }
}
