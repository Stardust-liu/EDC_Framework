using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class OptionItemToggleInfo
{
    public Action<int> changeIndex;
    public string[] toggleInfo;
    public int index;

    public OptionItemToggleInfo( Action<int> _changeIndex, string[] _toggleInfo, int _index)
    {
        changeIndex = _changeIndex;
        toggleInfo = _toggleInfo;
        index = _index;
    }
}

public class OptionItemToggle : BaseOptionItem
{
    public TextMeshProUGUI title;
    public Button rightBtn;
    public Button LeftBtn;
    public bool isLoopToggle;
    private OptionItemToggleInfo info;

    public void SetInitAction(OptionItemToggleInfo _optionItemToggleInfo, Action<Transform, string[]> _changeOptionItem, Action _exitBtn)
    {
        rightBtn.onClick.AddListener(ClickAddIndex);
        LeftBtn.onClick.AddListener(ClickSubIndex);
        info = _optionItemToggleInfo;
        SetInitAction(_changeOptionItem, _exitBtn);
        SetTextInfo();
    }

    public void UpdateToggleInfo(string[] toggleInfo)
    {
        info.toggleInfo = toggleInfo;
        SetTextInfo();
    }

    private void ClickAddIndex()
    {
        var toggleInfoCount = info.toggleInfo.Length - 1;
        var index = info.index;
        var tempIndex = index;
        if (isLoopToggle)
        {
            index = (index == toggleInfoCount) ? 0 : index + 1;
        }
        else
        {
            index = Mathf.Min(index + 1, toggleInfoCount);
        }
        info.index = index;
        ChangeIndex(tempIndex);
    }
    
    private void ClickSubIndex()
    {
        var toggleInfoCount = info.toggleInfo.Length - 1;
        var index = info.index;
        var tempIndex = index;
        if (isLoopToggle)
        {
            index = (index == 0) ? toggleInfoCount : index - 1;
        }
        else
        {
            index = Mathf.Max(index - 1, 0);
        }
        info.index = index;
        ChangeIndex(tempIndex);
    }

    private void ChangeIndex(int tempIndex)
    {
        if (tempIndex != info.index)
        {
            info.changeIndex.Invoke(info.index);
            SetTextInfo();
        }
    }

    private void SetTextInfo()
    {
        title.text = info.toggleInfo[info. index];
    }
}
