
public interface IBaseWindow : IBaseUI{}
public class BaseWindow<Model> : BaseUI<Model>, IBaseWindow where Model : BaseUI_Model, new()
{
    protected override void MoveToShowParent()
    {
        MoveToParent(Hub.Window, isShow: true);
    }

    protected override void MoveToHideParent()
    {
        MoveToParent(Hub.Window, isShow: false);
    }
}
public class BaseWindow : BaseUI, IBaseWindow
{
    protected override void MoveToShowParent()
    {
        MoveToParent(Hub.Window, isShow: true);
    }

    protected override void MoveToHideParent()
    {
        MoveToParent(Hub.Window, isShow: false);
    }
}