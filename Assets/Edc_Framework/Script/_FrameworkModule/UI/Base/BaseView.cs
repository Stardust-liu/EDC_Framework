using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class BaseViewExtensions{
    /// <summary>
    /// 返回上一个视图页面
    /// </summary>
    public static void BackLastView(this IBaseView baseViewExtensions){
        Hub.View.BackLastView();
    }
}

public interface IBaseView{}

public class BaseView<Model> : BaseUI<Model> ,IBaseView where Model : BaseUI_Model, new(){}

public class BaseView : BaseUI, IBaseView{}
