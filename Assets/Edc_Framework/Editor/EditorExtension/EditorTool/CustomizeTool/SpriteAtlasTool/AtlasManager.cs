using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "AtlasManager", menuName = "创建.Assets文件/CustomizeTool/图集工具/AtlasManager")]
public class AtlasManager : SerializedScriptableObject
{
    public SpriteAtlas[] SpriteAtlasList;

    [HorizontalGroup("split/left")]
    [Button("检查图集间的重复素材", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void CheckAtlasforDuplicateAssets(){
        var result = new Dictionary<Sprite, List<string>>();
        foreach (var item in SpriteAtlasList)
        {
            var spriteObject = item.GetPackables();
            foreach (var spriteObj in spriteObject)
            {
                var sprite = spriteObj as Sprite;
                if(!result.ContainsKey(sprite)){
                    result.Add(sprite, new List<string>());
                }
                result[sprite].Add(item.name);
            }   
        }
        foreach (var item in result)
        {
            string atlasName = null;
            if (item.Value.Count > 1)
            {
                Debug.Log("重复文件" + item.Key.name + "路径" + AssetDatabase.GetAssetPath(item.Key));
                foreach (var name in item.Value)
                {
                    atlasName += name + "  ";
                }
            }
            if(!string.IsNullOrEmpty(atlasName)){
                Debug.Log("重复图集" + atlasName);
            }
        }
    }

    [HorizontalGroup("split", 0.5f)]
    [Button("清空数据", ButtonSizes.Large), GUIColor(1f, 0.5f, 0.5f)]
    private void ClearData(){
        SpriteAtlasList = new SpriteAtlas[]{};
    }
}
