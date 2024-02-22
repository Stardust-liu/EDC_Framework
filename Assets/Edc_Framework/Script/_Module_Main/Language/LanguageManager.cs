using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;

public enum LanguageId{
    Chinese = 0,
    English = 1,
}
public class LanguageManager
{
    public LanguageId CurrentLanguage{get; private set;}
    public int SupportLanguageCount{get; private set;}
	private readonly HashSet<LanguageId> supportLange;

    public LanguageManager(){
        var languageIdArray = Enum.GetValues(typeof(LanguageId));
        SupportLanguageCount = languageIdArray.Length;
		supportLange = new HashSet<LanguageId>(SupportLanguageCount);
        foreach (LanguageId item in languageIdArray)
        {
            supportLange.Add(item);
        }
		LanguageInit();
	}

    /// <summary>
	/// 语言初始化
	/// </summary>
	private void LanguageInit(){
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.ChineseTraditional:
                CurrentLanguage = LanguageId.Chinese;
                break;
            default:
                CurrentLanguage = (LanguageId)Application.systemLanguage;
                break;
        }
		if (!supportLange.Contains(CurrentLanguage)){
			CurrentLanguage = LanguageId.English;
		}
	}

    /// <summary>
	/// 改变语言
	/// </summary>
	public void ChangeLange(LanguageId languageId)
    {
		Hub.EventCenter.OnEvent(EventName.changeLanguage, languageId);
		CurrentLanguage = languageId;
	}
}
