using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Character{
    None = 0,
    EWUnitSoldier,//电子战小组组长
    AirDefenseTeam,//防空小组士兵
    ATSoldier,//反坦克小组
    TankSoldier,//T90车长
    Radio,//无线电
    Karina,//卡琳娜少校
}

public class CharacterNameData : ParsCsv<CharacterNameData>
{
    private Dictionary<Character,string> characterNameData = new Dictionary<Character, string>();
    public CharacterNameData() : base(LoadAssetFile.CharacterInfo.characterFile){}

    protected override void SetData(string key)
    {
        characterNameData.Add((Character)Enum.Parse(typeof(Character), key), GetString(key,"Name"));
    }

    /// <summary>
    /// 获取角色名字
    /// </summary>
    public string GetCharacterName(string characterName){
        return characterNameData[(Character)Enum.Parse(typeof(Character), characterName)];
    }
    
    /// <summary>
    /// 获取角色名字
    /// </summary>
    public string GetCharacterName(Character characterName){
        return characterNameData[characterName];
    }
}
