using System;
using System.Collections;
using UnityEngine;

public class DialogueView_M
{
    private DialogueModule dialogueModule;

    public DialogueView_M(){
        //dialogueModule = ;
    }
    

    /// <summary>
    /// 获得当前对话角色
    /// </summary>
    public Character GetDialogueCharacter(){
        return dialogueModule.GetDialogueCharacter();
    }

    /// <summary>
    /// 获得对话角色名字
    /// </summary>
    public string GetDialogueCharacterName(){
        return dialogueModule.GetDialogueCharacterName();
    }

    /// <summary>
    /// 获得对话内容
    /// </summary>
    public string GetDialogueContent(){
        return dialogueModule.GetDialogueContent();
    }

    /// <summary>
    /// 获得对话背景
    /// </summary>
    public Sprite GetBg(){
        return dialogueModule.GetBg();
    }

    /// <summary>
    /// 获得对话角色站位
    /// </summary>
    public DialogueCharacterInfoVO[] GetCharacterSpriteInfo(){
        return dialogueModule.GetCharacterSpriteInfo();
    }

    /// <summary>
    /// 获得对话表现形式
    /// </summary>
    public PerformanceType GetPerformanceType(){
        return dialogueModule.GetPerformanceType();
    }

    /// <summary>
    /// 获得对话音效
    /// </summary>
    public AudioClip GetSoundEffect(){
        return dialogueModule.GetSoundEffect();
    }

    /// <summary>
    /// 获得对话声音
    /// </summary>
    public AudioClip GetSoundDialogue(){
        return dialogueModule.GetSoundDialogue();
    }

    /// <summary>
    /// 获得对话背景音
    /// </summary>
    public AudioClip GetSoundBg(){
        return dialogueModule.GetSoundBg();
    }
}
