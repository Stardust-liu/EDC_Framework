using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
[System.Serializable]
public class TextureQualitySetting{
    [LabelText("RGBA"),BoxGroup("优先使用")]
    public TextureImporterFormat RGBA_Main;
    [LabelText("RGB"),BoxGroup("优先使用")]
    public TextureImporterFormat RGB_Main;
    [LabelText("RGBA"),BoxGroup("备选")]
    public TextureImporterFormat RGBA_Alt;
    [LabelText("RGB"),BoxGroup("备选")]
    public TextureImporterFormat RGB_Alt;
}

[System.Serializable]
public class TexturePlatformSetting{
    [FoldoutGroup("高质量压缩格式"), HideLabel]
    public TextureQualitySetting highQuality;
    [FoldoutGroup("中等质量压缩格式"), HideLabel]
    public TextureQualitySetting MiddleQuality;
    [FoldoutGroup("低质量压缩格式"), HideLabel]
    public TextureQualitySetting lowQuality;
}


[CreateAssetMenu(fileName = "TextureSettingTool", menuName = "创建.Assets文件/素材设置工具/TextureSettingTool")]
public class TextureSettingTool : SerializedScriptableObject
{
    [TabGroup("PC",TextColor ="blue"),HideLabel]
    public TexturePlatformSetting pc = new TexturePlatformSetting();
    [TabGroup("Android",TextColor ="green"),HideLabel] 
    public TexturePlatformSetting android;
    [TabGroup("Ios",TextColor ="orange"),HideLabel] 
    public TexturePlatformSetting ios;
    [TabGroup("Switch",TextColor ="red"),HideLabel] 
    public TexturePlatformSetting nintendoSwitch;

    [LabelText("高质量图像路径"),FolderPath,BoxGroup("路径设置")]
    public List<string> highQualityPath;
    [LabelText("中等质量图像路径"),FolderPath,BoxGroup("路径设置")]
    public List<string> middleQualityPath;
    [LabelText("低质量图像路径"),FolderPath,BoxGroup("路径设置")]
    public List<string> lowQualityPath;

    [LabelText("优化白名单"),HideLabel]
    public Texture2D[] whitelist;
    [LabelText("使用备选格式的素材列表"),HideLabel, ReadOnly, GUIColor(1, 0.7f, 0.7f)]
    public List<Texture2D> settingFailList;
    private static List<Texture2D> staticSettingFailList;

    private readonly static HashSet<TextureImporterFormat> multiplesOf4Format = new (){
        TextureImporterFormat.BC7,
        TextureImporterFormat.BC5,
        TextureImporterFormat.BC6H,
        TextureImporterFormat.BC4,
        TextureImporterFormat.DXT5,
        TextureImporterFormat.DXT5Crunched,
        TextureImporterFormat.DXT1,
        TextureImporterFormat.DXT1Crunched,
        TextureImporterFormat.ETC2_RGB4,
        TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA,
        TextureImporterFormat.ETC2_RGBA8,
        TextureImporterFormat.ETC2_RGBA8Crunched,
        TextureImporterFormat.ETC_RGB4,
        TextureImporterFormat.ETC_RGB4Crunched,
    };

    private readonly static HashSet<TextureImporterFormat> potSizeFormat = new(){
        TextureImporterFormat.PVRTC_RGB2,
        TextureImporterFormat.PVRTC_RGB4,
        TextureImporterFormat.PVRTC_RGBA2,
        TextureImporterFormat.PVRTC_RGBA4,
    };

    private static bool isAlpha;
    private static TexturePlatformSetting textureSetting;
    private static HashSet<string> whiteListPath;

    private void SetWhiteListPath(){
        if(whitelist != null){
            var count = whitelist.Length;
            whiteListPath = new HashSet<string>();
            for (var i = 0; i < count; i++)
            {
                var path = AssetDatabase.GetAssetPath(whitelist[i]);
                if(!whiteListPath.Contains(path)){
                    whiteListPath.Add(path);
                }
                else{
                    Debug.LogWarning($"优化白名单素材资源重复，素材为 {whitelist[i]}");
                }
            }
        }
    }

    [HorizontalGroup("split/left")]
    [Button("设置图片为指定格式", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void ApplicationSetting(){
#if UNITY_STANDALONE
        textureSetting = pc;
#elif UNITY_ANDROID
        textureSetting = android;
#elif UNITY_IOS
        textureSetting = ios;
#elif UNITY_SWITCH
        textureSetting = nintendoSwitch;
#endif
        staticSettingFailList ??= new List<Texture2D>();
        staticSettingFailList.Clear();
        settingFailList = staticSettingFailList;
        SetWhiteListPath();
        FindTexture(highQualityPath, HighQualitySetting);
        FindTexture(middleQualityPath, MiddleQualitySetting);
        FindTexture(lowQualityPath, LowQualitySetting);
    }

    [HorizontalGroup("split", 0.5f)]
    [Button("清空使用备选格式的素材列表", ButtonSizes.Large), GUIColor(1, 0.5f, 0.5f)]
    private void ClickCleanList()
    {
        staticSettingFailList?.Clear();
        Debug.Log("已清空");
    }
    private static void FindTexture(List<string> pathList, Action<Texture2D, TextureImporter, string> setting){
        if(pathList == null)
            return;
        var count = pathList.Count;
        for (var i = 0; i < count; i++)
        {
            FindTexture(pathList[i], setting);
        }

        static void FindTexture(string path, Action<Texture2D, TextureImporter, string> setting){
            if(AssetDatabase.IsValidFolder(path)){
                string[] imagePaths = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
                foreach (string item in imagePaths)
                {
                    if(whiteListPath.Contains(item)) continue;
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(item);
                    setting.Invoke(texture, CheckIsAlpha(item), item);
                }
            }
            else{
                Debug.LogError($"不存在 {path} 路径");
            }
        }
    }

    private static void HighQualitySetting(Texture2D texture, TextureImporter textureImporter, string path){
        SetTextureSetting(textureImporter, GetSuitableFormat(texture,textureSetting.highQuality), path);
    }

    private static void MiddleQualitySetting(Texture2D texture, TextureImporter textureImporter, string path){
        SetTextureSetting(textureImporter, GetSuitableFormat(texture, textureSetting.MiddleQuality), path);
    }

    private static void LowQualitySetting(Texture2D texture, TextureImporter textureImporter, string path){
        SetTextureSetting(textureImporter, GetSuitableFormat(texture, textureSetting.lowQuality), path);
    }

    private static TextureImporter CheckIsAlpha(string path){
        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
        if (textureImporter.DoesSourceTextureHaveAlpha())
        {
            textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
            textureImporter.alphaIsTransparency = true;
            isAlpha = true;
        }
        else
        {
            textureImporter.alphaSource = TextureImporterAlphaSource.None;
            textureImporter.alphaIsTransparency = false;
            isAlpha = false;
        }
        return textureImporter;
    }

    /// <summary>
    /// 选择合适格式
    /// </summary>
    private static TextureImporterFormat GetSuitableFormat(Texture2D texture, TextureQualitySetting setting){
        TextureImporterFormat format; 
        if(isAlpha){
            format = IsContradictSize(texture, setting.RGBA_Main)? setting.RGBA_Main : setting.RGBA_Alt;
        }
        else{
            format = IsContradictSize(texture, setting.RGB_Main)? setting.RGB_Main : setting.RGB_Alt;
        }
        return format;
    }

    private static void SetTextureSetting(TextureImporter textureImporter, TextureImporterFormat format, string path){        
        TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings();
        settings.overridden = true;
        settings.format = format;
#if UNITY_STANDALONE
        settings.name = "Standalone";
#elif UNITY_ANDROID
        settings.name = "Android";
#elif UNITY_IOS
        settings.name = "iOS";
#elif UNITY_SWITCH
        settings.name = "Switch";
#endif
        textureImporter.SetPlatformTextureSettings(settings);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    /// <summary>
    /// 检查是否满足指定格式的尺寸
    /// </summary>
    private static bool IsContradictSize(Texture2D texture, TextureImporterFormat format){
        var isMultiplesOf4Format = multiplesOf4Format.Contains(format);
        var isPotSizeFormat = potSizeFormat.Contains(format);

        if((!isMultiplesOf4Format && !isPotSizeFormat) ||
           (isMultiplesOf4Format && CheckIs4multiplesSize(texture))||
           (isPotSizeFormat && CheckIsPotSize(texture)))
        {
            return true;
        }
        else
        {
            staticSettingFailList.Add(texture);
            return false;
        }
    }
   
    /// <summary>
    /// 检查图像长宽是否是4的倍率
    /// </summary>
    private static bool CheckIs4multiplesSize(Texture2D texture){
        if(texture.width% 4 == 0 && texture.height% 4 == 0){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 检查图像长宽是否是2的次幂
    /// </summary>
    private static bool CheckIsPotSize(Texture2D texture){
        if(IsPot(texture.width) && IsPot(texture.height)){
            return true;
        }
        else{
            return false;
        }
        static bool IsPot(int vale){
            return (vale & (vale - 1)) == 0;
        }
    }
}
