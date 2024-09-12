using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView_V : BaseUI_V
{
    public Image DialogueBg;
    public Image DialogueBgEffect;

    public Image[] dialogueCharacter;

    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueContent;
    public TextMeshProUGUI narrative_Character;
    public TextMeshProUGUI narrative_Environmental;

    public CanvasGroup characterParent;
    public CanvasGroup dialogueInfoParent;

    public Image transitionPanel;

    public RectTransform movieBordersUp;
    public RectTransform movieBordersDown;

    public RectTransform skipBtnRectTransform;
    public CanvasGroup skipBtnCanvasGroup;
    public CustomClickEffectsBtn skipBtn;
    public BaseBtn nextDialogueBtn;
}
