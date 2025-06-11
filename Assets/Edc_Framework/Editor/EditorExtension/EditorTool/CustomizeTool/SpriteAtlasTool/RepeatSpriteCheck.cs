using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "RepeatSpriteCheck", menuName = "创建.Assets文件/CustomizeTool/图集工具/RepeatSpriteCheck")]
public class RepeatSpriteCheck : SerializedScriptableObject
{
    public SpriteAtlas[] SpriteAtlasList;

    [HorizontalGroup("split/left")]
    [Button("移除图集中的重复素材", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void RemoveAtlasRepeatSprite(){

        foreach (var item in SpriteAtlasList)
        {
            RemoveRepeatSprite(item);
        }
    }

    [HorizontalGroup("split", 0.5f)]
    [Button("清空列表数据", ButtonSizes.Large), GUIColor(1f, 0.5f, 0.5f)]
    private void ClearData(){
        SpriteAtlasList = new SpriteAtlas[]{};
    }

    private void RemoveRepeatSprite(SpriteAtlas atlas){
        foreach (var item in SpriteAtlasList)
        {
            var addAtlasSprite = new List<Object>();
            var RemoveAtlasSpriteCount = 0;
            var spriteObject = item.GetPackables();
            foreach (var spriteObj in spriteObject)
            {   
                if(!addAtlasSprite.Contains(spriteObj)){
                    addAtlasSprite.Add(spriteObj);
                }
                else{
                    RemoveAtlasSpriteCount++;
                }
            }   
            if(RemoveAtlasSpriteCount != 0){
                atlas.Remove(atlas.GetPackables());
                atlas.Add(addAtlasSprite.ToArray());
                BuildNewAtlas(atlas);
            }
        }
    }

    private void BuildNewAtlas(SpriteAtlas atlas){
        BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
        SpriteAtlasUtility.PackAtlases(new SpriteAtlas[]{atlas}, currentBuildTarget);
    }
}
