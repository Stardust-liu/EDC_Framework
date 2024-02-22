using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetFile
{
    private static ABManager aBManager;
    public static void Init(){
        aBManager = Hub.Ab;
        aBManager.LoadAbFile("scriptableobject");
    }

    private static CharacterTextSetting characterInfo;
    public static CharacterTextSetting CharacterInfo{
        get{
            if(characterInfo == null){
                characterInfo = Resources.Load<CharacterTextSetting>("AssetFile/CharacterTextSetting");
                //characterInfo = aBManager.LoadScriptableobject<CharacterTextSetting>("CharacterTextSetting");
            }
            return characterInfo;
        }
    }
}
