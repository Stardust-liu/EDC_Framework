using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageTabInfoManager{
    public class LanguageTabInfo : ParsCsv<LanguageTabInfo>
    {
        protected override void SetData(string key)
        {
            var count = Hub.Language.SupportLanguageCount;
            var langeInfoArray = new string[count];
            for (int i = 0; i < count; i++)
            {
                langeInfoArray[i] = GetString(key, Enum.GetName(typeof(LanguageId), i));
            }
            AddInfo(textAsset, key, langeInfoArray);
        } 
    }
    private static readonly Dictionary<TextAsset, Dictionary<string , string[]>> languageInfo = new();
    public static void ParseData(TextAsset csv){
        if(!languageInfo.ContainsKey(csv)){
            languageInfo.Add(csv, new Dictionary<string, string[]>());
            LanguageTabInfo.Instance.ParseData(csv);
        }
    }

    public static void AddInfo(TextAsset csv, string key, string[] value){
        languageInfo[csv].Add(key, value);
    }

    public static string GetData(TextAsset csv, string key, int value){
        return languageInfo[csv][key][value];
    }

    public static void DestroyInfo(TextAsset csv){
        languageInfo.Remove(csv);
    }
}

[ShowOdinSerializedPropertiesInInspector]
public class LanguageTextUpdate : MonoBehaviour, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
    public struct LanguageTextInfo{
        public TextMeshProUGUI text;
        public string id;
    }

    [LabelText("翻译文件")]
    public TextAsset languageTab;

    [LabelText("静态文字列表")]
    public LanguageTextInfo[] languageTextUpdate;
    private bool isAddListener;
    private LanguageId currentTextLanguage = LanguageId.Chinese;

    private void OnEnable() {
        if(FrameworkManager.isInitFinish){
            if(!isAddListener){
                LanguageTabInfoManager.ParseData(languageTab);
                Hub.EventCenter.AddListener<LanguageId>(EventName.changeLanguage, SetLanguage);
                isAddListener = true;
            }
            if(currentTextLanguage != Hub.Language.CurrentLanguage){
                SetLanguage(Hub.Language.CurrentLanguage);
            }
        }
    }

    private void OnDisable()
    {   
        if(FrameworkManager.isInitFinish && isAddListener){
            Hub.EventCenter.RemoveListener<LanguageId>(EventName.changeLanguage, SetLanguage);
            isAddListener = false;
        }
    }

    private void OnDestroy() {
        if(FrameworkManager.isInitFinish && isAddListener){
            LanguageTabInfoManager.DestroyInfo(languageTab);
            Hub.EventCenter.RemoveListener<LanguageId>(EventName.changeLanguage, SetLanguage);
            isAddListener = false;
        }
    }

    private void SetLanguage(LanguageId languageId){
        foreach (var item in languageTextUpdate)
        {
            item.text.text = LanguageTabInfoManager.GetData(languageTab, item.id, (int)languageId);
        }
        currentTextLanguage = languageId;
    }

    [SerializeField, HideInInspector]
    private SerializationData serializationData;
    SerializationData ISupportsPrefabSerialization.SerializationData 
    { 
        get { return serializationData; } 
        set { serializationData = value; } 
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref serializationData);
    }
}
