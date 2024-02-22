using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum PerformanceType{
    None,
    Environmental, //环境漫画呈现
    Character,//人物漫画呈现
}

public class DialogueCharacter{
    public Character character;
    public Sprite sprite;
}

public class DialogueData{
    public Character dialogueCharacter;
    public string dialogueCharacterName;
    public string dialogueContent;
    public Sprite bg;
    public DialogueCharacter[] characterSpriteInfo;
    public AudioClip soundEffect;
    public AudioClip soundDialogue;
    public AudioClip soundBg;
    public PerformanceType performanceType;
}


public class DialogueFileData : ParsCsv<DialogueFileData>
{
    public Dictionary<int, DialogueData> dialogueData = new Dictionary<int, DialogueData>();
    private PerformanceType lastPerformanceType;
    private static TextAsset csv;
    public static void SetCsvFile(TextAsset csv){
        DialogueFileData.csv = csv;
    }
    public DialogueFileData() : base(csv){}

    public void ParseNewData(){
        dialogueData.Clear();
        ParseData(csv);
    }

    protected override void SetData(string key)
    {
        var data = new DialogueData();
        var CharacterData = GetString(key,"Character");
        var dialogueContentData = GetString(key,"DialogueContent");
        var bgData = GetString(key,"DialogueBG");
        var characterSpritePath_1 = GetString(key,"Character_1");
        var characterSpritePath_2 = GetString(key,"Character_2");
        var characterSpritePath_3 = GetString(key,"Character_3");
        var performanceTypeData = GetString(key,"PerformanceType");
        var soundEffectData = GetString(key,"SFX");
        var soundDialogueData = GetString(key,"VO");
        var soundBgData = GetString(key,"BGM");

        if(!IsNullOrEmpty(CharacterData)){
            data.dialogueCharacter = (Character)Enum.Parse(typeof(Character), CharacterData);
            data.dialogueCharacterName = CharacterNameData.Instance.GetCharacterName(data.dialogueCharacter);
        }
        if(!IsNullOrEmpty(dialogueContentData)){
            data.dialogueContent = dialogueContentData;
        }
        if(!IsNullOrEmpty(bgData)){
            if(bgData == "Transparent"){
                data.bg = DialogueView_M.TransparentSprite;
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

        var characterSpriteInfo = new DialogueCharacter[3];
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
        dialogueData.Add(int.Parse(key), data);

        static bool IsNullOrEmpty(string Data){
            return string.IsNullOrEmpty(Data);
        }

        static bool IsUniversalPath(string path){
            return path.StartsWith("U_");
        }

        static DialogueCharacter GetCharacterSprite(string path){
            var dialogueCharacter = new DialogueCharacter();
            if(path == "Transparent"){
                dialogueCharacter.sprite = DialogueView_M.TransparentSprite;
            }
            else{
                dialogueCharacter.character = (Character)Enum.Parse(typeof(Character), path.Substring(0, path.LastIndexOf('/')));
                dialogueCharacter.sprite = Resources.Load<Sprite>($"Character/{path}");
            }
            return dialogueCharacter;
        }
    }
}

public class DialogueView_M
{
    private readonly CharacterTextSetting characterInfo;
    private DialogueFileData currentDialogueFileData;
    private Dictionary<int, DialogueData> dialogueData;
    private DialogueData currentDialogueData;
    private int maxCurrentDialogueID;
    public Color DefaultColor{get; private set;}
    public int CurrentDialogueID {get; private set;}
    public bool IsProhibitEndDialogue{get; private set;}
    public bool IsEndDialogue {get; private set;}
    public static Sprite TransparentSprite {get; private set;}
    public WaitForSeconds DisplaySpeed {get; private set;}
    public IEnumerator typeWriterEffect;


    public DialogueView_M(){
        characterInfo = LoadAssetFile.CharacterInfo;
        DisplaySpeed = new WaitForSeconds(0.05f);
        DefaultColor = new Color(0.5f, 0.5f, 0.5f, 1);
        TransparentSprite = Resources.Load<Sprite>($"Dialogue/Universal/transparent");
    }

    public void InitDialogue(string dialogueSceneName){
        DialogueFileData.SetCsvFile(characterInfo.GetDialogueFile(dialogueSceneName));
        if(currentDialogueFileData == null){
            currentDialogueFileData = DialogueFileData.Instance;
        }
        else{
            currentDialogueFileData.ParseNewData();
        }
        dialogueData = currentDialogueFileData.dialogueData;
        currentDialogueData = dialogueData[1];
        maxCurrentDialogueID = dialogueData.Count;
        CurrentDialogueID = 1;
    }

    /// <summary>
    /// 开始对话
    /// </summary>
    public void StartDialogue(){
        IsEndDialogue = false;
        IsProhibitEndDialogue = true;
        FrameworkManager.instance.StartCoroutine(WaitCanEndDialogue());
        IEnumerator WaitCanEndDialogue(){
            yield return WaitTime.GetWait(WaitTime.slow);
            IsProhibitEndDialogue = false;
        }
    }
    
    /// <summary>
    /// 结束对话
    /// </summary>
    public void EndDialogue(){
        IsEndDialogue = true;
        typeWriterEffect = null;
    }

    /// <summary>
    /// 下一段对话
    /// </summary>
    public void NextDialogue(){
        CurrentDialogueID++;
        currentDialogueData = dialogueData[CurrentDialogueID];
    }

    /// <summary>
    /// 是否是最后一句对话
    /// </summary>
    public bool IsFinishDialogue(){
        return CurrentDialogueID == maxCurrentDialogueID;
    }

    /// <summary>
    /// 获得当前对话角色
    /// </summary>
    public Character GetDialogueCharacter(){
        return currentDialogueData.dialogueCharacter;
    }

    /// <summary>
    /// 获得对话角色名字
    /// </summary>
    public string GetDialogueCharacterName(){
        return currentDialogueData.dialogueCharacterName;
    }

    /// <summary>
    /// 获得对话内容
    /// </summary>
    public string GetDialogueContent(){
        return currentDialogueData.dialogueContent;
    }

    /// <summary>
    /// 获得对话背景
    /// </summary>
    public Sprite GetBg(){
        return currentDialogueData.bg;
    }

    /// <summary>
    /// 获得对话角色
    /// </summary>
    public DialogueCharacter[] GetCharacterSpriteInfo(){
        return currentDialogueData.characterSpriteInfo;
    }

    /// <summary>
    /// 获得对话音效
    /// </summary>
    public AudioClip GetSoundEffect(){
        return currentDialogueData.soundEffect;
    }

    /// <summary>
    /// 获得对话声音
    /// </summary>
    public AudioClip GetSoundDialogue(){
        return currentDialogueData.soundDialogue;
    }

    /// <summary>
    /// 获得对话背景音
    /// </summary>
    public AudioClip GetSoundBg(){
        return currentDialogueData.soundBg;
    }

    /// <summary>
    /// 获得对话表现形式
    /// </summary>
    public PerformanceType GetPerformanceType(){
        return currentDialogueData.performanceType;
    }
}
