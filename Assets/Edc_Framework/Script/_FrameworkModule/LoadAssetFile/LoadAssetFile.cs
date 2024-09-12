using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOFileManager
{
    private static ABManager aBManager;
    public static void Init(){
        aBManager = Hub.Ab;
        aBManager.LoadAbFile("scriptableobject");
    }

    private CharacterTextSetting characterInfo;
    public CharacterTextSetting CharacterInfo{
        get{
            if(characterInfo == null){
                characterInfo = Resources.Load<CharacterTextSetting>("AssetFile/CharacterTextSetting");
                //characterInfo = aBManager.LoadScriptableobject<CharacterTextSetting>("CharacterTextSetting");
            }
            return characterInfo;
        }
    }
}
