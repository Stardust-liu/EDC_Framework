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

[CreateAssetMenu(fileName = "ChangeFontTool", menuName = "创建Assets文件/ChangeFontTool")]
public class ChangeFontTool : SerializedScriptableObject
{
    public List<GameObject> prefabList;
    [HideIf("textType", TextType.Text)]
    public TMP_FontAsset targetFont_Plugin;
    [ShowIf("textType", TextType.Text)]
    public Font targetfont_Native;
    public Material material;
    public TextType textType;

    [HorizontalGroup("split", 0.35f)]
    [Button("清空列表", ButtonSizes.Large), GUIColor(0.8f, 0.3f, 0.3f)]
    private void ClickCleanPrefabList()
    {
        prefabList.Clear();
    }

    [HorizontalGroup("split/right")]
    [Button("修改字体", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1)]
    public void ChangeFont()
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
}
