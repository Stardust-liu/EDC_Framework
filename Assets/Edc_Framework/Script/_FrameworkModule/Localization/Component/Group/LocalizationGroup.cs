using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public enum LocalizationType{
    Text,
    Asset,
    Size,
}

public interface IEditorChangeLange{
    void ChangeLanguage(SystemLanguage languageId);
}

public class LocalizationGroup : MonoBehaviour, IEditorChangeLange, IAutoBindEvent
{
    [LabelText("本地化Item列表")]
    public BaseLocalization[] localizationItem;
    private bool init;
    private void OnEnable()
    {
        if (FrameworkManager.isInitFinish && init)
        {
            RefreshContent();
        }
    }

    private void Start()
    {
        this.AddListener_StartDestroy<ChangeLanguage>(ChangeLanguage, gameObject);
        RefreshContent();
        init = true;
    }

    public void ChangeLanguage(ChangeLanguage changeLanguage)
    {
        if (gameObject.activeInHierarchy) {
            RefreshContent();
        }
    }

    /// <summary>
    /// 通过编辑器工具修改语言
    /// </summary>
    void IEditorChangeLange.ChangeLanguage(SystemLanguage languageId)
    {
        RefreshContent();
    }

    private void RefreshContent(){
        foreach (var item in localizationItem)
        {
            item.RefreshContent();
        }
    }
}
