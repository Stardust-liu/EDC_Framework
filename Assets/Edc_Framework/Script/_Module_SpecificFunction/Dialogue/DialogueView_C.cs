using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class DialogueView_C : View_MVC<DialogueView_M, DialogueView_V, DialogueView_C>
{
    public override void SetPrefabInfo()
    {
        SetPrefabInfo("DialogueView");
    }

    protected override void PrepareForShwo(){
        base.PrepareForShwo();
        view_V.transitionPanel.enabled = false;
        view_V.movieBordersUp.anchoredPosition = Vector2.up * 138.3f;
        view_V.movieBordersDown.anchoredPosition = Vector2.down * 138.3f;
        view_V.skipBtnRectTransform.anchoredPosition = new Vector2(-77.5f, -125);
        view_V.skipBtnCanvasGroup.alpha = 0;
        view_V.gameObject.SetActive(true);

        view_V.movieBordersUp.DOAnchorPosY(0, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        view_V.movieBordersDown.DOAnchorPosY(0, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        view_V.skipBtnRectTransform.DOAnchorPosX(-335,WaitTime.slow).SetEase(Ease.OutQuart).OnComplete(ShwoFinish);
        view_V.skipBtnCanvasGroup.DOFade(1,WaitTime.mediumSpeed);
    }  

    protected override void PrepareForHide(){
        base.PrepareForHide();
        view_V.movieBordersUp.DOAnchorPosY(138.3f, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        view_V.movieBordersDown.DOAnchorPosY(-138.3f, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        view_V.skipBtnRectTransform.DOAnchorPosX(77.5f,WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        view_V.skipBtnCanvasGroup.DOFade(0,WaitTime.mediumSpeed).OnComplete(HideFinish);
        view_V.dialogueInfoParent.DOFade(0, WaitTime.fast);
        view_V.characterParent.DOFade(0, WaitTime.fast);
    }

    protected override void HideFinish()
    {
        base.HideFinish();
        view_V.gameObject.SetActive(true);
    }


    public override void CreateUiPrefab()
    {
        base.CreateUiPrefab();
        view_V.skipBtn.onClickEvent.AddListener(ClickSkip);
        view_V.nextDialogueBtn.onClickEvent.AddListener(ClickNextDialogue);
    }

    /// <summary>
    /// 点击跳过
    /// </summary>
    private void ClickSkip(){
        FinishDialogue();
    }

    /// <summary>
    /// 点击显示下一段对话内容
    /// </summary>
    private void ClickNextDialogue(){
        if(view_M.IsProhibitEndDialogue){
            return;
        }
        var audio = Hub.Audio;
        audio.StopCurrentSoundEffect(false);
        audio.StopCurrentSoundDialogue(false);
        if(!view_M.IsEndDialogue){
            CommonTool.StopTypeWriterEffect(view_M.typeWriterEffect, view_M.EndDialogue);
            DisplayAllContent(view_M.GetDialogueContent());
        }
        else{
            if(view_M.IsFinishDialogue()){
                FinishDialogue();
            }
            else{
                view_M.NextDialogue();
                SetDialogueInfo();
            }
        }
    }

    /// <summary>
    /// 完成对话
    /// </summary>
    private void FinishDialogue(){
        Debug.Log("完成对话");
    }


    /// <summary>
    /// 初始化一段对话
    /// </summary>
    public static void InitDialogue(string dialogueSceneName){
        Hub.View.ChangeView(Instance);
        view_M.InitDialogue(dialogueSceneName);
        view_V.dialogueName.text = null;
        view_V.dialogueContent.text = null;
        view_V.narrative_Character.text = null;
        view_V.narrative_Environmental.text = null;
        for (int i = 0; i < 3; i++)
        {
            view_V.dialogueCharacter[i].sprite = DialogueView_M.TransparentSprite;
        }
        SetDialogueInfo(true);
    }

    /// <summary>
    /// 设置对话内容
    /// </summary>
    private static void SetDialogueInfo(bool isInit = false){
        var performanceType = view_M.GetPerformanceType();
        SetPerformanceType(performanceType, isInit);
        SetDialogueName(view_M.GetDialogueCharacterName());
        SetDialogueContent(view_M.GetDialogueContent(),performanceType);
        SetBg(view_M.GetBg());
        SetDialogueCharacter(view_M.GetCharacterSpriteInfo());
        PlaySoundBg(view_M.GetSoundBg());
        PlaySoundEffect(view_M.GetSoundEffect());
        PlaySoundDialogue(view_M.GetSoundDialogue());
    }

    /// <summary>
    /// 设置对话角色名字
    /// </summary>
    private static void SetDialogueName(string name){
        view_V.dialogueName.text = name;
    }

    /// <summary>
    /// 设置对话内容
    /// </summary>
    private static void SetDialogueContent(string content, PerformanceType performanceType){
        if(content != null){
            view_M.StartDialogue();
            TextMeshProUGUI textState = null;
            if(performanceType == PerformanceType.Environmental){
                textState = view_V.narrative_Environmental;
            }
            else if(performanceType == PerformanceType.Character){
                if(view_V.dialogueName.text == null){
                    view_V.narrative_Character.enabled = true;
                    view_V.dialogueContent.enabled = false;
                    textState = view_V.narrative_Character;
                }
                else{
                    view_V.dialogueContent.enabled = true;
                    view_V.narrative_Character.enabled = false;
                    textState = view_V.dialogueContent;
                }
            }
            view_M.typeWriterEffect = CommonTool.TypeWriterEffect(textState, content, view_M.DisplaySpeed, view_M.EndDialogue);
        }
        else{
            if(performanceType == PerformanceType.Environmental){
                view_V.narrative_Environmental.text = null;
            }
            else if(performanceType == PerformanceType.Character){
                view_V.narrative_Character.text = null;         
                view_V.dialogueContent.text = null;
            }
        }
    }

    private static void DisplayAllContent(string content){
        view_V.dialogueContent.text = content;
    }
    

    /// <summary>
    /// 修改背景
    /// </summary>
    private static void SetBg(Sprite bg){
        if(bg != null){
            view_V.DialogueBg.sprite = bg;
        }
    }

    /// <summary>
    /// 设置对话角色站位
    /// </summary>
    private static void SetDialogueCharacter(DialogueCharacter[] dialogueCharacter){
        Sprite dialogueCharacterSprite;
        for (int i = 0; i < 3; i++)
        {
            if(dialogueCharacter[i] == null){
                view_V.dialogueCharacter[i].enabled = false;
            }
            else{
                view_V.dialogueCharacter[i].enabled = true;
                dialogueCharacterSprite = dialogueCharacter[i].sprite;
                if(dialogueCharacterSprite != null){
                    view_V.dialogueCharacter[i].sprite = dialogueCharacterSprite;
                    if(dialogueCharacter[i].character == view_M.GetDialogueCharacter()){
                        view_V.dialogueCharacter[i].color = Color.white;
                    }
                    else{
                        view_V.dialogueCharacter[i].color = view_M.DefaultColor;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    private static void PlaySoundBg(AudioClip audio){
        if(audio != null){
            Hub.Audio.PlaysoundBg(audio);
        }
    }   

    /// <summary>
    /// 播放音效
    /// </summary>
    private static void PlaySoundEffect(AudioClip audio){
        if(audio != null){
            Hub.Audio.PlaySoundEffect(audio);
        }
    }

    /// <summary>
    /// 播放对话
    /// </summary>
    private static void PlaySoundDialogue(AudioClip audio){
        if(audio != null){
            Hub.Audio.PlaySoundDialogue(audio);
        }
    }

    /// <summary>
    /// 设置对话表现形式
    /// </summary>
    private static void SetPerformanceType(PerformanceType performanceType, bool isInit){
        if(isInit && performanceType == PerformanceType.None){
            LogManager.LogError("对话文件配置错误，没有配置初始状态");
            return;
        }
        switch (performanceType)
        {
            case PerformanceType.Environmental:
                EnvironmentalState(isInit);
            break;
            case PerformanceType.Character:
                CharacterState(isInit);
            break;
        }
    }

    
    /// <summary>
    /// 环境漫画模式
    /// </summary>
    private static void EnvironmentalState(bool isImmediatelyChange){
        if(isImmediatelyChange){
            view_V.dialogueInfoParent.alpha = 0;
            view_V.characterParent.alpha = 0;
        }
        else{
            view_V.dialogueInfoParent.DOFade(0, WaitTime.fast);
            view_V.characterParent.DOFade(0, WaitTime.fast);
        }
        view_V.narrative_Environmental.enabled = true;
    }

    /// <summary>
    /// 角色对话模式
    /// </summary>
    private static void CharacterState(bool isImmediatelyChange){
        if(isImmediatelyChange){
            view_V.dialogueInfoParent.alpha = 1;
            view_V.characterParent.alpha = 1;
        }
        else{
            view_V.dialogueInfoParent.DOFade(1, WaitTime.fast);
            view_V.characterParent.DOFade(1, WaitTime.fast);
        }
        view_V.narrative_Environmental.enabled = false;
    }

    /// <summary>
    /// 过渡效果
    /// </summary>
    private static void Transition(TweenCallback callBack){
        view_V.transitionPanel.enabled = true;
        var doQuence = DOTween.Sequence();
        doQuence.Append(view_V.transitionPanel.DOFade(1,WaitTime.mediumSpeed).SetLoops(1,LoopType.Yoyo).SetEase(Ease.OutQuad));
        doQuence.InsertCallback(WaitTime.mediumSpeed, callBack);
    }
}
