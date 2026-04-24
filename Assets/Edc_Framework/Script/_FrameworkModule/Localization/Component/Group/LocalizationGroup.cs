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

    /// <summary>
    /// 数据加载完成
    /// </summary>
    public void Init()
    {
        init = true;
    }

    /// <summary>
    /// 重置数据加载状态
    /// </summary>
    public void Reset()
    {
        init = false;
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public void RefreshContent(){
        if (gameObject.activeInHierarchy)
        {
            foreach (var item in localizationItem)
            {
                item.RefreshContent();
            }
        }
    }


    /// <summary>
    /// 通过编辑器工具修改语言
    /// </summary>
    void IEditorChangeLange.ChangeLanguage(SystemLanguage languageId)
    {
        RefreshContent();
    }
}
