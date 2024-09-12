using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class TextureResources : AssetPostprocessor
{   
    private const string postprocessorRangePath = "Assets/Game/Sources";    

    public void OnPostprocessTexture(Texture2D texture)
    {
        if (!assetPath.StartsWith(postprocessorRangePath)){
            return;
        }
        var assetNameCutEnd = assetPath.LastIndexOf('/') + 1;
        var assetName = assetPath.Substring(assetNameCutEnd, assetPath.LastIndexOf('.') - assetNameCutEnd);
        var suffixNameCutEnd = assetName.LastIndexOf('_');
        if(suffixNameCutEnd != -1){
            var identifierName = assetName.Substring(suffixNameCutEnd);
            var textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = SetTextureType(identifierName);
        }
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
}
