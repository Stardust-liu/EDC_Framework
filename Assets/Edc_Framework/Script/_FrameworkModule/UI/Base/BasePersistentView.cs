using Sirenix.OdinInspector;
using UnityEngine;

public interface IBasePersistentView{}
public class BasePersistentView<Model> : BaseUI<Model>, IBasePersistentView where Model : BaseUI_Model, new()
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