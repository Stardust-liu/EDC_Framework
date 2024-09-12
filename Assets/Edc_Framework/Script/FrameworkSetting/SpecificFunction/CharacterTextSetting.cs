using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterText", menuName = "创建.Assets文件/CharacterText")]
public class CharacterTextSetting : SerializedScriptableObject
{
    [LabelText("对话文件管理")]
    [DictionaryDrawerSettings(KeyLabel = "对话演出名称", ValueLabel ="对话文件")]
    public Dictionary<string, TextAsset> dialogueFileDictionary;

    [LabelText("角色名字文件管理")]
    public TextAsset characterFile;

    public TextAsset GetDialogueFile(string dialogueSceneName){
        return dialogueFileDictionary[dialogueSceneName];
    }
}
