
using Sirenix.OdinInspector;
using UnityEngine;

public interface IBaseView : IBaseUI {}

/// <summary>
/// 空 UI Model，占位用。
/// 当某个泛型 UI 基类需要填写 Model 类型，但实际界面不需要 Model 时，使用该类作为泛型参数。
/// </summary>
public class EmptyModel : BaseUI_Model
{
    protected override void Init(){}
}
public class BaseView<Model> : BaseView where Model : BaseUI_Model, new()
{
    protected Model model;
    protected override void Init()
    {
        base.Init();
        if (typeof(Model) == typeof(EmptyModel))
        {
            return;
        }
        model = CreateModel<Model>();
    }

    protected override void DestroyPanel()
    {
        model = null;
        base.DestroyPanel();
    }
}
public class BaseView : BaseUI, IBaseView
{
    protected override void MoveToShowParent()
    {
        MoveToParent(Hub.View, isShow: true);
    }

    protected override void MoveToHideParent()
    {
        MoveToParent(Hub.View, isShow: false);
    }
}
