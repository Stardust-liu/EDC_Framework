using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "CreateAtlas", menuName = "创建.Assets文件/CustomizeTool/图集工具/CreateAtlas")]
public class CreateAtlasSetting : SerializedScriptableObject
{
    [LabelText("图集保存通用路径")]
    [ReadOnly, ShowInInspector]
    private readonly string generalpath = "Assets/Game/Sources/Atlas/";

    [LabelText("提取预制体使用的图像资源并打包成图集")]
    [DictionaryDrawerSettings(KeyLabel = "合并后的图集名字", ValueLabel = "图集列表")]
    public Dictionary<string, GameObject> prefabs = new();

    [LabelText("将图像资源列表打包成图集")]
    [DictionaryDrawerSettings(KeyLabel = "合并后的图集名字", ValueLabel = "Sprite列表")]
    public Dictionary<string, Sprite[]> apriteAtlasDictionary = new();
    
    [LabelText("将多个图集合并为新图集")]
    [DictionaryDrawerSettings(KeyLabel = "合并后的图集名字", ValueLabel = "图集列表")]
    public Dictionary<string, SpriteAtlas[]> mergeAtlasDictionary = new();

    [HorizontalGroup("split/left")]
    [Button("创建图集", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void ClickCreateAtlas(){
        foreach (var item in prefabs)
        {
            var prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(item.Value);
            GetPrefabSprite(prefabInstance.GetComponentsInChildren<SpriteRenderer>(), item.Key);
            DestroyImmediate(prefabInstance);
        }
        foreach (var item in mergeAtlasDictionary)
        {
            MergeAtlas(item.Value, item.Key);
        }
        foreach (var item in apriteAtlasDictionary)
        {
            SetSpriteList(item.Value, item.Key);
        }
    }

    [HorizontalGroup("split", 0.5f)]
    [Button("清空列表数据", ButtonSizes.Large), GUIColor(1f, 0.5f, 0.5f)]
    private void ClearData(){
        prefabs.Clear();
        mergeAtlasDictionary.Clear();
        apriteAtlasDictionary.Clear();
    }

    private void GetPrefabSprite(SpriteRenderer[] sprirerenderArray, string prefabName){
        var addAtlasSprite = new List<Sprite>();
        foreach (var item in sprirerenderArray)
        {
            if(!addAtlasSprite.Contains(item.sprite)){
                addAtlasSprite.Add(item.sprite);
            }
        }
        CreateAtlas(addAtlasSprite, prefabName);
    }

    private void MergeAtlas(SpriteAtlas[] spriteAtlasArray, string spriteAtlasName){
        var addAtlasSprite = new List<Sprite>();
        foreach (var item in spriteAtlasArray)
        {
            var spriteObject = item.GetPackables();
            foreach (var spriteObj in spriteObject)
            {   
                var sprite = spriteObj as Sprite;
                if(!addAtlasSprite.Contains(sprite)){
                    addAtlasSprite.Add(sprite);
                }
            }   
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
        }
        CreateAtlas(addAtlasSprite, spriteAtlasName);
    }

    private void SetSpriteList(Sprite[] sprites, string spriteAtlas){
        var addAtlasSprite = new List<Sprite>();
        foreach (var item in sprites)
        {
            if(!addAtlasSprite.Contains(item)){
                addAtlasSprite.Add(item);
            }
        }
        CreateAtlas(addAtlasSprite, spriteAtlas);
    }


    private void CreateAtlas(List<Sprite> addAtlasSprite, string spriteAtlas){
        if(addAtlasSprite.Count <= 1){
            return;
        }
        var atlas = new SpriteAtlas();
        var packingSettings = new SpriteAtlasPackingSettings()
        {
            enableTightPacking = false,
            padding = 2
    
        };
        var textureSettings = new SpriteAtlasTextureSettings()
        {
            sRGB = true,
            generateMipMaps = false,
            filterMode = FilterMode.Bilinear
        };
        
        atlas.SetPackingSettings(packingSettings);
        atlas.SetTextureSettings(textureSettings);
        

        atlas.Add(new List<Object>(addAtlasSprite).ToArray());
        SaveAtlas(atlas, spriteAtlas);
    }
    
    private void SaveAtlas(SpriteAtlas atlas, string spriteAtlasName){
        var folderPath = generalpath;
        var atlasPath = $"{folderPath}/{spriteAtlasName}.spriteatlas";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        if(File.Exists(atlasPath)){
            AssetDatabase.DeleteAsset(atlasPath);
        }
        AssetDatabase.CreateAsset(atlas, atlasPath);
        AssetDatabase.SaveAssets();
        BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
        SpriteAtlasUtility.PackAtlases(new SpriteAtlas[]{atlas}, currentBuildTarget);
    }
}
