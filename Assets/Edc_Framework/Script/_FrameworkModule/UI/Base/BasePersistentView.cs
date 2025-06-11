using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class BasePersistentViewExtensions{
    /// <summary>
    /// 关闭自身面板
    /// </summary>
    public static void CloseSelf<T>(this IBasePersistentView baseViewExtensions)where T : BasePersistentView
    {
        Hub.PersistentView.ClosePersistentView<T>();
    }
}

public interface IBasePersistentView{}

public class BasePersistentView<Model> : BaseUI<Model>, IBasePersistentView where Model : BaseUI_Model, new(){}
public class BasePersistentView : BaseUI, IBasePersistentView{}