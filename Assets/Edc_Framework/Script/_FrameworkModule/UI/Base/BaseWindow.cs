
public interface IBaseWindow : IBaseUI, IWindowUI{}
public class BaseWindow<Model> : BaseWindow where Model : BaseUI_Model, new()
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
public class BaseWindow : BaseUI, IBaseWindow
{
    void IWindowUI.Cover()
    {
        OnCover();
    }
    
    void IWindowUI.Reveal()
    {
        OnReveal();
    }
    protected override void MoveToShowParent()
    {
        MoveToParent(Hub.Window, isShow: true);
    }

    protected override void MoveToHideParent()
    {
        MoveToParent(Hub.Window, isShow: false);
    }

    /// <summary>
    /// 遮挡
    /// </summary>
    protected virtual void OnCover(){}

    /// <summary>
    /// 恢复
    /// </summary>
    protected virtual void OnReveal(){}
}