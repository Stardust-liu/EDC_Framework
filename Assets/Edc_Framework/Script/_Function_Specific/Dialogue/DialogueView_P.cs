using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DialogueView_P : View_MVP<DialogueView_M, DialogueView_V, DialogueView_P>
{
    private DialogueModule dialogueModule;
    public override void CreateUiPrefab()
    {
        CreateUiPrefab("DialogueView");
        //dialogueModule = 
    }

    protected override void StartShow()
    {
        base.StartShow();
        DialogueModule.eventCenter.AddListener(DialogueModuleEventName.DisplayAllContent, DisplayAllContent);
        DialogueModule.eventCenter.AddListener(DialogueModuleEventName.FinishDialogue, FinishDialogue);
        DialogueModule.eventCenter.AddListener(DialogueModuleEventName.SetNextDialogueInfo, SetNextDialogueInfo);
    }

    protected override void StartHide()
    {
        base.StartHide();
        DialogueModule.eventCenter.RemoveListener(DialogueModuleEventName.DisplayAllContent, DisplayAllContent);
        DialogueModule.eventCenter.RemoveListener(DialogueModuleEventName.FinishDialogue, FinishDialogue);
        DialogueModule.eventCenter.RemoveListener(DialogueModuleEventName.SetNextDialogueInfo, SetNextDialogueInfo);
    }

    protected override void ShowFinish()
    {
        base.ShowFinish();
        SetDialogueInfo(true);
    }

    /// <summary>
    /// 点击跳过
    /// </summary>
    public void ClickSkip(){
        FinishDialogue();
    }

    /// <summary>
    /// 处理对话时的点击操作
    /// </summary>
    public void HandleDialogueClick(){
        dialogueModule.HandleDialogueClick();
    }

    /// <summary>
    /// 显示当前所有对话内容
    /// </summary>
    private void DisplayAllContent(){
        view_V.DisplayAllContent(view_M.GetDialogueContent());
    }

    /// <summary>
    /// 完成对话
    /// </summary>
    private void FinishDialogue(){
        Hub.View.BackLastView();
    }

    /// <summary>
    /// 设置对话信息
    /// </summary>
    private void SetNextDialogueInfo(){
        SetDialogueInfo(false);
    }

    /// <summary>
    /// 设置对话信息
    /// </summary>
    private void SetDialogueInfo(bool isInit){
        view_V.SetBg(view_M.GetBg());
        view_V.SetDialogueName(view_M.GetDialogueCharacterName());
        view_V.SetDialogueCharacter(view_M.GetCharacterSpriteInfo(), view_M.GetDialogueCharacter());
        view_V.SetPerformanceType(view_M.GetPerformanceType(), isInit);

        var dialogueCharacterName = view_M.GetDialogueCharacterName();
        var dialogueContent = view_M.GetDialogueContent();
        var soundBg = view_M.GetSoundBg();
        var soundEffect = view_M.GetSoundEffect();
        var soundDialogue = view_M.GetSoundDialogue();
        if(soundBg != null){
            Hub.Audio.PlaysoundBg(soundBg);
        }
        if(soundEffect != null){
            Hub.Audio.PlaySoundEffect(soundEffect);
        }
        if(soundDialogue != null){
            Hub.Audio.PlaySoundDialogue(soundDialogue);
        }
    }
}
