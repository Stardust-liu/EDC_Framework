using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerformanceType{
    None,
    Environmental, //环境漫画呈现
    Character,//人物漫画呈现
}

public class DialogueInfoCfg : ParsCsv<DialogueInfoCfg>
{
    private PerformanceType lastPerformanceType;
    private readonly Dictionary<int, DialogueDataInfoVO> dialogueData = new();
    private static TextAsset csv;

    public void SetCsvFile(TextAsset csv){
        DialogueInfoCfg.csv = csv;
    }

    public void ParseNewData(){
        dialogueData.Clear();
        ParseData(csv);
    }

    protected override void SetData()
    {
        var data = new DialogueDataInfoVO();
        var CharacterData = GetString("Character");
        var dialogueContentData = GetString("DialogueContent");
        var bgData = GetString("DialogueBG");
        var characterSpritePath_1 = GetString("Character_1");
        var characterSpritePath_2 = GetString("Character_2");
        var characterSpritePath_3 = GetString("Character_3");
        var performanceTypeData = GetString("PerformanceType");
        var soundEffectData = GetString("SFX");
        var soundDialogueData = GetString("VO");
        var soundBgData = GetString("BGM");

        if(!IsNullOrEmpty(CharacterData)){
            data.dialogueCharacter = (Character)Enum.Parse(typeof(Character), CharacterData);
            data.dialogueCharacterName = CharacterNameCfg.Instance.GetCharacterName(data.dialogueCharacter);
        }
        if(!IsNullOrEmpty(dialogueContentData)){
            data.dialogueContent = dialogueContentData;
        }
        if(!IsNullOrEmpty(bgData)){
            if(bgData == "Transparent"){
                data.bg = Resources.Load<Sprite>($"Dialogue/Universal/Transparent");
            }
            else{
                if(IsUniversalPath(bgData)){
                    data.bg = Resources.Load<Sprite>($"Dialogue/Universal/DialogueBG/{bgData}");
                }
                else{
                    data.bg = Resources.Load<Sprite>($"Dialogue/{csv.name}/DialogueBG/{bgData}");
                }
            }
        }

        var characterSpriteInfo = new DialogueCharacterInfoVO[3];
        characterSpriteInfo[0] = !IsNullOrEmpty(characterSpritePath_1)? GetCharacterSprite(characterSpritePath_1): null;
        characterSpriteInfo[1] = !IsNullOrEmpty(characterSpritePath_2)? GetCharacterSprite(characterSpritePath_2): null;
        characterSpriteInfo[2] = !IsNullOrEmpty(characterSpritePath_3)? GetCharacterSprite(characterSpritePath_3): null;
        data.characterSpriteInfo = characterSpriteInfo;

        if(!IsNullOrEmpty(performanceTypeData)){
            data.performanceType = (PerformanceType)Enum.Parse(typeof(PerformanceType), performanceTypeData);
            lastPerformanceType = data.performanceType;
        }
        else{
            data.performanceType = lastPerformanceType;
        }

        if(!IsNullOrEmpty(soundEffectData)){
            if(IsUniversalPath(soundEffectData)){
                data.soundEffect = Resources.Load<AudioClip>($"Dialogue/Universal/Audio/SFX/{soundEffectData}");
            }
            else{
                data.soundEffect = Resources.Load<AudioClip>($"Dialogue/{csv.name}/Audio/SFX/{soundEffectData}");
            }
        }
        if(!IsNullOrEmpty(soundDialogueData)){
            if(IsUniversalPath(soundDialogueData)){
                data.soundDialogue = Resources.Load<AudioClip>($"Dialogue/Universal/Audio/VO/{soundDialogueData}");
            }
            else{
                data.soundDialogue = Resources.Load<AudioClip>($"Dialogue/{csv.name}/Audio/VO/{soundDialogueData}");
            }
        }
        if(!IsNullOrEmpty(soundBgData)){
            if(IsUniversalPath(soundBgData)){
                data.soundBg = Resources.Load<AudioClip>($"Dialogue/Universal/Audio/BGM/{soundBgData}");
            }
            else{
                data.soundBg = Resources.Load<AudioClip>($"Dialogue/{csv.name}/Audio/BGM/{soundBgData}");
            }
        }
        dialogueData.Add(CurrentReadDataRow, data);

        static bool IsNullOrEmpty(string Data){
            return string.IsNullOrEmpty(Data);
        }

        static bool IsUniversalPath(string path){
            return path.StartsWith("U_");
        }

        static DialogueCharacterInfoVO GetCharacterSprite(string path){
            var dialogueCharacter = new DialogueCharacterInfoVO();
            if(path == "Transparent"){
                dialogueCharacter.sprite = Resources.Load<Sprite>($"Dialogue/Universal/Transparent");
            }
            else{
                dialogueCharacter.character = (Character)Enum.Parse(typeof(Character), path.Substring(0, path.LastIndexOf('/')));
                dialogueCharacter.sprite = Resources.Load<Sprite>($"Character/{path}");
            }
            return dialogueCharacter;
        }
    }

    /// <summary>
    /// 获取指定对话次数的数据
    /// </summary>
    public DialogueDataInfoVO GetDialogueData(int currentDialogueCount){
        return dialogueData[currentDialogueCount];
    }

    /// <summary>
    /// 获取当前文本的对话次数
    /// </summary>
    public int GetMaxDialogueCount(){
        return dialogueData.Count-1;
    }
}

public class DialogueCharacterInfoVO{
    public Character character;
    public Sprite sprite;
}

public class DialogueDataInfoVO{
    public Character dialogueCharacter;
    public string dialogueCharacterName;
    public string dialogueContent;
    public Sprite bg;
    public DialogueCharacterInfoVO[] characterSpriteInfo;
    public AudioClip soundEffect;
    public AudioClip soundDialogue;
    public AudioClip soundBg;
    public PerformanceType performanceType;
}
