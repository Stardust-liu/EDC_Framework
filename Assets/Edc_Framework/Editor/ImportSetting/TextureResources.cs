using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class TextureResources : AssetPostprocessor
{
    private const string postprocessorRangePath = "Assets/Game/Sources";
    private const string highQualityPath1 = "Assets/Game/Sources/Sprite/HighQuality";
    private const string highQualityPath2 = "Assets/Game/Sources/Resources/Dialogue";//默认对话模块的素材都是高质量的
    private const string highQualityPath3 = "Assets/Game/Sources/Resources/Character";//默认角色立绘的素材都是高质量的

    private bool isHighQuality;
    private bool isTransparency;
    

    public void OnPostprocessTexture(Texture2D texture)
    {
        if (!assetPath.StartsWith(postprocessorRangePath)){
            return;
        }

        int assetNameCutEnd = assetPath.LastIndexOf('/') + 1;
        string assetName = assetPath.Substring(assetNameCutEnd, assetPath.LastIndexOf('.') - assetNameCutEnd);
        string identifierName = null;
        int suffixNameCutEnd = assetName.LastIndexOf('_');
        if(suffixNameCutEnd != -1){
            identifierName = assetName.Substring(suffixNameCutEnd);
        }
        isHighQuality = assetPath.StartsWith(highQualityPath1)? true : false;
        isHighQuality = !isHighQuality? assetPath.StartsWith(highQualityPath2) : true;
        isHighQuality = !isHighQuality? assetPath.StartsWith(highQualityPath3) : true;

        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureType = SetTextureType(identifierName);
        if (textureImporter.DoesSourceTextureHaveAlpha())
        {
            textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
            textureImporter.alphaIsTransparency = true;
            isTransparency = true;
        }
        else
        {
            textureImporter.alphaSource = TextureImporterAlphaSource.None;
            textureImporter.alphaIsTransparency = false;
            isTransparency = false;
        }
         
        TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings();
        settings.overridden = true;
#if UNITY_STANDALONE
        settings.name = "Standalone";
#elif UNITY_ANDROID
        settings.name = "Android";
#elif UNITY_IOS
        settings.name = "iOS";
#endif
        if(!CheckIsMultipleOfFour(texture)){
            Debug.Log($"图像:{texture.name} 像素大小需要是4的倍数,以便进行压缩");
            textureImporter.SetPlatformTextureSettings(settings);
            return;
        }
        else{
            if (isHighQuality)
            {
                settings.format = SetHighQualityFormat();
            }
            else{
                if(isTransparency){
                    settings.format = SetLowQualityTransparencyFormat();
                }
                else{
                    settings.format = SetLowQualityOpaqueFormat();
                }
            }
        }
        textureImporter.SetPlatformTextureSettings(settings);
    }

    /// <summary>
    /// 设置图片种类
    /// </summary>
    private TextureImporterType SetTextureType(string suffixName)
    {
        switch (suffixName)
        {
            case "_NM":
                return TextureImporterType.NormalMap;
            default:
                return TextureImporterType.Sprite;
        }
    }

    /// <summary>
    /// 设置高质量图片格式
    /// </summary>
    private TextureImporterFormat SetHighQualityFormat(){
#if UNITY_STANDALONE
        return TextureImporterFormat.BC7;
#elif UNITY_ANDROID
        return TextureImporterFormat.ASTC_4x4;
#elif UNITY_IOS
        return TextureImporterFormat.ASTC_4x4;
#endif
    }

    /// <summary>
    /// 设置低质量透明图片格式
    /// </summary>
    private TextureImporterFormat SetLowQualityTransparencyFormat(){
#if UNITY_STANDALONE
        return TextureImporterFormat.DXT5;
#elif UNITY_ANDROID
        return TextureImporterFormat.ETC2_RGBA8;
#elif UNITY_IOS
        return TextureImporterFormat.PVRTC_RGBA4;
#endif
    }

    /// <summary>
    /// 设置低质量不透明图片格式
    /// </summary>
    private TextureImporterFormat SetLowQualityOpaqueFormat(){
#if UNITY_STANDALONE
        return TextureImporterFormat.DXT1;
#elif UNITY_ANDROID
        return TextureImporterFormat.ETC_RGB4;
#elif UNITY_IOS
        return TextureImporterFormat.PVRTC_RGB4;
#endif
    }

    /// <summary>
    /// 检查导入的图像是否是4的倍率
    /// </summary>
    private bool CheckIsMultipleOfFour(Texture2D texture){
        if(texture.width% 4 == 0 && texture.height% 4 == 0){
            return true;
        }
        else{
            return false;
        }
    }
}
