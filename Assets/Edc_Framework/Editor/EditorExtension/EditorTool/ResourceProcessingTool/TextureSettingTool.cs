using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum TextureSize{
    _32 = 32,
    _64 = 64,
    _128 = 128,
    _256 = 256,
    _512 = 512,
    _1024 = 1024,
    _2048 = 2048,
    _4096 = 4096,
}

public enum SizeReduceGrade{
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _3 = 3,
    _4 = 4,
    _5 = 5,
}

[System.Serializable]
public class CompressionInfo{
    [LabelText("是否进行压缩")]
    public bool isOn = true;
    [LabelText("禁止修改透明通道设置")]
    public bool isIgnoreAlphaSetting;
    [LabelText("强制设置图像尺寸")]
    public bool isForcedSettingSize;
    public string path;
    [ShowIf("isForcedSettingSize"), LabelText("图像尺寸")]
    public TextureSize textureSize = TextureSize._1024;
    [HideIf("isForcedSettingSize"), LabelText("压缩等级")]
    public SizeReduceGrade SizeReduceGrade = SizeReduceGrade._0;
}

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

[CreateAssetMenu(fileName = "TextureSettingTool", menuName = "OS/素材设置工具/TextureSettingTool")]
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

    [LabelText("高质量图像"),BoxGroup("路径设置")]
    public CompressionInfo[] highQualityPath;
    [LabelText("中等质量图像"),BoxGroup("路径设置")]
    public CompressionInfo[] middleQualityPath;
    [LabelText("低质量图像"),BoxGroup("路径设置")]
    public CompressionInfo[] lowQualityPath;

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

    private readonly static TextureSize[] textureSizeArray = new TextureSize[]{
        TextureSize._32,
        TextureSize._64,
        TextureSize._128,
        TextureSize._256,
        TextureSize._512,
        TextureSize._1024,
        TextureSize._2048,
        TextureSize._4096,
    };

    private static bool isAlpha;
    private static TexturePlatformSetting textureSetting;

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
        FindTexture("压缩高质量图像中", highQualityPath, HighQualitySetting);
        FindTexture("压缩中等质量图像中", middleQualityPath, MiddleQualitySetting);
        FindTexture("压缩低等质量图像中", lowQualityPath, LowQualitySetting);
        EditorUtility.ClearProgressBar();
        Debug.Log("压缩完成");
    }

    [HorizontalGroup("split", 0.5f)]
    [Button("清空使用备选格式的素材列表", ButtonSizes.Large), GUIColor(1, 0.5f, 0.5f)]
    private void ClickCleanList()
    {
        settingFailList?.Clear();
        staticSettingFailList?.Clear();
        Debug.Log("已清空");
    }
    private static void FindTexture(string title, CompressionInfo[] compressionInfo, Action<Texture2D, TextureImporter, string, int> setting){
        if(compressionInfo == null)
            return;
        
        foreach (var item in compressionInfo)
        {
            if(item.isOn){
                FindTexture(title, item, setting);
            }
        }

        static void FindTexture(string title, CompressionInfo compressionInfo, Action<Texture2D, TextureImporter, string, int> setting){
            var path = compressionInfo.path;
            if(AssetDatabase.IsValidFolder(path)){
                string[] imagePaths = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
                var count = imagePaths.Length;
                var index = 0f;
                foreach (string item in imagePaths)
                {
                    index++;
                    EditorUtility.DisplayProgressBar(title, item, index/count);
                    try
                    {
                        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(item);
                        if(staticSettingFailList.Contains(texture))continue;
                        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(item);
                        int maxTextureSize = GetTextureSize(compressionInfo, texture);
                        if(!compressionInfo.isIgnoreAlphaSetting){
                            CheckIsHaveAlpha(textureImporter);
                        }
                        setting.Invoke(texture, textureImporter, item, maxTextureSize);
                    }
                    catch{
                        Debug.LogError($"{item}加载失败，可能图片已经损坏");
                    }
                    
                }
            }
            else{
                Debug.LogError($"不存在 {path} 路径");
            }
        }
    }

    private static void HighQualitySetting(Texture2D texture, TextureImporter textureImporter, string path, int maxTextureSize){
        SetTextureSetting(textureImporter, GetSuitableFormat(texture,textureSetting.highQuality), path, maxTextureSize);
    }

    private static void MiddleQualitySetting(Texture2D texture, TextureImporter textureImporter, string path, int maxTextureSize){
        SetTextureSetting(textureImporter, GetSuitableFormat(texture, textureSetting.MiddleQuality), path, maxTextureSize);
    }

    private static void LowQualitySetting(Texture2D texture, TextureImporter textureImporter, string path, int maxTextureSize){
        SetTextureSetting(textureImporter, GetSuitableFormat(texture, textureSetting.lowQuality), path, maxTextureSize);
    }

    /// <summary>
    /// 检查是否包含透明通道
    /// </summary>
    private static TextureImporter CheckIsHaveAlpha(TextureImporter textureImporter){
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

    /// <summary>
    /// 获取图像压缩尺寸
    /// </summary>
    private static int GetTextureSize(CompressionInfo compressionInfo, Texture2D texture){
        if(compressionInfo.isForcedSettingSize){
            return (int)compressionInfo.textureSize;
        }
        else{
            var maxSideLength = Mathf.Max(texture.width, texture.height);
            var index = 0;
            foreach (var item in textureSizeArray)
            {
                if(maxSideLength <= (int)item){
                    break;
                }
                index++;
            }
            return (int)textureSizeArray[Mathf.Max(index - (int)compressionInfo.SizeReduceGrade, 0)];
        }
    }

    private static void SetTextureSetting(TextureImporter textureImporter, TextureImporterFormat format, string path, int maxTextureSize){        
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
        settings.maxTextureSize = maxTextureSize;
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