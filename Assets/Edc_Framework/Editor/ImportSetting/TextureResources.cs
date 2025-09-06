using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class TextureResources : AssetPostprocessor
{
    private HashSet<string> postprocessorRangePath = new()
    {
        "Assets/Game/Sources",
        "Assets/DemoExample/Sources"
    };


    public void OnPostprocessTexture(Texture2D texture)
    {
        var isCanPostprocess = false;
        foreach (var item in postprocessorRangePath)
        {
            if (assetPath.StartsWith(item))
            {
                isCanPostprocess = true;
                break;
            }
        }
        if (!isCanPostprocess)
        {
            return;
        }
        var assetNameCutEnd = assetPath.LastIndexOf('/') + 1;
        var assetName = assetPath.Substring(assetNameCutEnd, assetPath.LastIndexOf('.') - assetNameCutEnd);
        var suffixNameCutEnd = assetName.LastIndexOf('_');
        var identifierName = "";
        if (suffixNameCutEnd != -1)
        {
            identifierName = assetName.Substring(suffixNameCutEnd);

        }
        var textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureType = SetTextureType(identifierName);
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
