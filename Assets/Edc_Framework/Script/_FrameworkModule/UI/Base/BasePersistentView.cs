using Sirenix.OdinInspector;
using UnityEngine;

public interface IBasePersistentView : IBaseUI{}
public class BasePersistentView<Model> : BasePersistentView where Model : BaseUI_Model, new()
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
public class BasePersistentView : BaseUI, IBasePersistentView
{
    protected override void MoveToShowParent()
    {
        MoveToParent(Hub.PersistentView, isShow: true);
    }

    protected override void MoveToHideParent()
    {
        MoveToParent(Hub.PersistentView, isShow: false);
    }
}