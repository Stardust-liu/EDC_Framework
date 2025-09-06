
public interface IBaseView : IBaseUI { }
public class BaseView<Model> : BaseUI<Model>, IBaseView where Model : BaseUI_Model, new()
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
