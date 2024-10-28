using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLocalization : MonoBehaviour
{
    [SerializeField]
    protected string id;
    private static LocalizationManager localization;
    protected static LocalizationManager Localization{
        get{
            localization ??= Hub.Localization;
            return localization;
        }
    }
    
    /// <summary>
    /// 更新ID
    /// </summary>
    public void ChangeID(string _id){
        id = _id;
    }

    /// <summary>
    /// 刷新本地化内容
    /// </summary>
    public abstract void RefreshContent();
    
    /// <summary>
    /// 刷新本地化内容（自定义ID）
    /// </summary>
    public virtual void RefreshContent(string _id, bool isOverrideID){
        if(isOverrideID){
            id = _id;
        }
    } 
}
