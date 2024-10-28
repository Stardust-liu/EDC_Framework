using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueModuleEventName{
    /// <summary>
    /// 显示所有对话内容
    /// </summary>
    public const string DisplayAllContent = nameof(DisplayAllContent);

    /// <summary>
    /// 完成对话
    /// </summary>
    public const string FinishDialogue = nameof(FinishDialogue);

    /// <summary>
    /// 设置下一段对话内容
    /// </summary>
    public const string SetNextDialogueInfo = nameof(SetNextDialogueInfo);
}

public class DialogueModule : MonoBehaviour
{
    private int maxDialogueCount;
    public Color DefaultColor{get; private set;}
    public int CurrentDialogueCount {get; private set;}
    public bool IsProhibitEndDialogue{get; private set;}
    public bool IsEndDialogue {get; private set;}
    public static Sprite TransparentSprite {get; private set;}
    public WaitForSeconds DisplaySpeed {get; private set;}

    private DialogueDataInfoVO currentDialogueData;
    private readonly CharacterTextSetting characterInfo;
    private readonly DialogueInfoCfg dialogueInfoCfg;
    public static readonly EventCenter eventCenter = new();

    public DialogueModule(){
        characterInfo = Hub.Resources.GetScriptableobject<CharacterTextSetting>("CharacterTextSetting");
        dialogueInfoCfg = DialogueInfoCfg.Instance;
        DisplaySpeed = new WaitForSeconds(0.05f);
        DefaultColor = new Color(0.5f, 0.5f, 0.5f, 1);
        TransparentSprite = Resources.Load<Sprite>($"Dialogue/Universal/transparent");
    }

    /// <summary>
    /// 初始化对话
    /// </summary>
    public void InitDialogue(string dialogueSceneName){
        dialogueInfoCfg.SetCsvFile(characterInfo.GetDialogueFile(dialogueSceneName));
        dialogueInfoCfg.ParseNewData();
        CurrentDialogueCount = 0;
        currentDialogueData = dialogueInfoCfg.GetDialogueData(CurrentDialogueCount);
        maxDialogueCount = dialogueInfoCfg.GetMaxDialogueCount();
        Hub.View.ChangeView(DialogueView_P.Instance);
    }

    /// <summary>
    /// 开始对话
    /// </summary>
    public void StartDialogue(){
        IsEndDialogue = false;
        IsProhibitEndDialogue = true;
        StartCoroutine(WaitCanEndDialogue());
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
    }

    /// <summary>
    /// 下一段对话
    /// </summary>
    public void HandleDialogueClick(){
        if(IsProhibitEndDialogue){
            return;
        }
        var audio = Hub.Audio;
        audio.StopCurrentSoundEffect(false);
        audio.StopCurrentSoundDialogue(false);
        if(!IsEndDialogue){
            eventCenter.OnEvent(DialogueModuleEventName.DisplayAllContent);
        }
        else{
            if(CurrentDialogueCount == maxDialogueCount){
                eventCenter.OnEvent(DialogueModuleEventName.FinishDialogue);
            }
            else{
                CurrentDialogueCount++;
                currentDialogueData = dialogueInfoCfg.GetDialogueData(CurrentDialogueCount);
                eventCenter.OnEvent(DialogueModuleEventName.SetNextDialogueInfo);
            }
        }
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
    public DialogueCharacterInfoVO[] GetCharacterSpriteInfo(){
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


    /// <summary>
    /// 播放背景音
    /// </summary>
    private void PlaySoundBg(AudioClip audio){
        if(audio != null){
            Hub.Audio.PlaysoundBg(audio);
        }
    }   

    /// <summary>
    /// 播放音效
    /// </summary>
    private void PlaySoundEffect(AudioClip audio){
        if(audio != null){
            Hub.Audio.PlaySoundEffect(audio);
        }
    }

    /// <summary>
    /// 播放对话
    /// </summary>
    private void PlaySoundDialogue(AudioClip audio){
        if(audio != null){
            Hub.Audio.PlaySoundDialogue(audio);
        }
    }
}
