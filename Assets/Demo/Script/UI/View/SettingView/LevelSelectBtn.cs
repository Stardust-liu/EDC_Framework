using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectBtnInfo
{
    public int levelID;
    public string levelName;
    public LevelSelectBtnInfo(int _levelID, string _levelName)
    {
        levelID = _levelID;
        levelName = _levelName;
    }
}

public class LevelSelectBtn : BaseSelectItem
{
    private int levelID;
    public TextMeshProUGUI levelName;
    private Action<Transform, int> changeOptionItem;
    private Action exitBtn;

    public void SetInitAction(LevelSelectBtnInfo Info, Action<Transform, int> _changeOptionItem, Action _exitBtn)
    {
        changeOptionItem = _changeOptionItem;
        exitBtn = _exitBtn;
        levelID = Info.levelID;
        levelName.text = Info.levelName;
    }


    protected override void ChangeOptionItem()
    {
        changeOptionItem?.Invoke(selectEffectParent, levelID);
    }

    protected override void ExitBtn()
    {
        exitBtn?.Invoke();
    }
}
