
using Sirenix.OdinInspector;
using UnityEngine;

public interface IBaseView : IBaseUI {}
public class BaseView<Model> : BaseView where Model : BaseUI_Model, new()
{
    protected Model model;
    protected override void Init()
    {
        base.Init();
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
