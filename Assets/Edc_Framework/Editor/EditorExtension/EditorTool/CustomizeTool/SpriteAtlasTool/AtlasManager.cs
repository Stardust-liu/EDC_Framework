using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "AtlasManager", menuName = "创建.Assets文件/CustomizeTool/图集工具/AtlasManager")]
public class AtlasManager : SerializedScriptableObject
{
    public SpriteAtlas[] SpriteAtlasList;

    [HorizontalGroup("split/left")]
    [Button("检查包含重复素材的图集", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
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
            if(item.Value.Count > 1){
                string atlasName = "";
                foreach (var name in item.Value)
                {
                    atlasName +=name+"  ";
                }
                Debug.Log(atlasName);
                //Debug.Log(item.Key.name);
            }
        }
    }

    [HorizontalGroup("split", 0.5f)]
    [Button("清空数据", ButtonSizes.Large), GUIColor(1f, 0.5f, 0.5f)]
    private void ClearData(){
        SpriteAtlasList = new SpriteAtlas[]{};
    }
}
