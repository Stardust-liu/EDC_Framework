using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView_V : BaseUI_V<DialogueView_P>
{
    public Sprite transparentSprite;
    public Image DialogueBg;
    public Image DialogueBgEffect;
    public LocalizationText dialogueName;
    public Typewriter dialogueContent;
    public Typewriter narrative_Character;
    public Typewriter narrative_Environmental;

    public CanvasGroup characterParent;
    public CanvasGroup dialogueInfoParent;

    public Image transitionPanel;

    public RectTransform movieBordersUp;
    public RectTransform movieBordersDown;

    public RectTransform skipBtnRectTransform;
    public CanvasGroup skipBtnCanvasGroup;
    public CustomizeTweenBtn skipBtn;
    public CustomizeTweenBtn clickDialogueBtn;
    public Image[] dialogueCharacter;

    protected override void Start()
    {
        base.Start();
        skipBtn.onClick.AddListener(presenter.ClickSkip);
        clickDialogueBtn.onClick.AddListener(presenter.HandleDialogueClick);
    }

    public override void StartShow(){
        base.StartShow();
        transitionPanel.enabled = false;
        movieBordersUp.anchoredPosition = Vector2.up * 138.3f;
        movieBordersDown.anchoredPosition = Vector2.down * 138.3f;
        skipBtnRectTransform.anchoredPosition = new Vector2(-77.5f, -125);
        skipBtnCanvasGroup.alpha = 0;

        gameObject.SetActive(true);
        movieBordersUp.DOAnchorPosY(0, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        movieBordersDown.DOAnchorPosY(0, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        skipBtnRectTransform.DOAnchorPosX(-335,WaitTime.slow).SetEase(Ease.OutQuart);
        skipBtnCanvasGroup.DOFade(1,WaitTime.mediumSpeed);
    }  

    public override void StartHide(){
        movieBordersUp.DOAnchorPosY(138.3f, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        movieBordersDown.DOAnchorPosY(-138.3f, WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        skipBtnRectTransform.DOAnchorPosX(77.5f,WaitTime.mediumSpeed).SetEase(Ease.OutQuart);
        skipBtnCanvasGroup.DOFade(0,WaitTime.mediumSpeed);
        dialogueInfoParent.DOFade(0, WaitTime.fast);
        characterParent.DOFade(0, WaitTime.fast);
    }

    public void InitDialogue(){
        dialogueName.CleanTextContent();
        // dialogueContent.CleanTextContent();
        // narrative_Character.CleanTextContent();
        // narrative_Environmental.CleanTextContent();
        for (int i = 0; i < 3; i++)
        {
            dialogueCharacter[i].sprite = transparentSprite;
        }
    }

    /// <summary>
    /// 设置对话角色名字
    /// </summary>
    public void SetDialogueName(string name){
        dialogueName.RefreshContent(name);
    }
    
    /// <summary>
    /// 显示所有对话内容
    /// </summary>
    public void DisplayAllContent(string content){
        //dialogueContent.RefreshContent(content);
    }

    /// <summary>
    /// 修改背景
    /// </summary>
    public void SetBg(Sprite bg){
        if(bg != null){
            DialogueBg.sprite = bg;
        }
    }

    /// <summary>
    /// 设置对话角色站位
    /// </summary>
    public void SetDialogueCharacter(DialogueCharacterInfoVO[] characterInfo, Character currentSpeaker){
        Sprite characterSprite;
        for (int i = 0; i < 3; i++)
        {
            if(characterInfo[i] == null){
                dialogueCharacter[i].enabled = false;
            }
            else{
                dialogueCharacter[i].enabled = true;
                characterSprite = characterInfo[i].sprite;
                if(characterSprite != null){
                    dialogueCharacter[i].sprite = characterSprite;
                    if(characterInfo[i].character == currentSpeaker){
                        dialogueCharacter[i].color = Color.white;
                    }
                    else{
                        dialogueCharacter[i].color = Color.clear;
                    }
                }
            }
        }
    }


    /// <summary>
    /// 设置对话表现形式
    /// </summary>
    public void SetPerformanceType(PerformanceType performanceType, bool isInit){
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
    private void EnvironmentalState(bool isImmediatelyChange){
        if(isImmediatelyChange){
            dialogueInfoParent.alpha = 0;
            characterParent.alpha = 0;
        }
        else{
            dialogueInfoParent.DOFade(0, WaitTime.fast);
            characterParent.DOFade(0, WaitTime.fast);
        }
        narrative_Environmental.gameObject.SetActive(true);
    }

    /// <summary>
    /// 角色对话模式
    /// </summary>
    private void CharacterState(bool isImmediatelyChange){
        if(isImmediatelyChange){
            dialogueInfoParent.alpha = 1;
            characterParent.alpha = 1;
        }
        else{
            dialogueInfoParent.DOFade(1, WaitTime.fast);
            characterParent.DOFade(1, WaitTime.fast);
        }
        narrative_Environmental.gameObject.SetActive(false);
    }

    /// <summary>
    /// 过渡效果
    /// </summary>
    private void Transition(TweenCallback callBack){
        transitionPanel.enabled = true;
        var doQuence = DOTween.Sequence();
        doQuence.Append(transitionPanel.DOFade(1,WaitTime.mediumSpeed).SetLoops(1,LoopType.Yoyo).SetEase(Ease.OutQuad));
        doQuence.InsertCallback(WaitTime.mediumSpeed, callBack);
    }
}
