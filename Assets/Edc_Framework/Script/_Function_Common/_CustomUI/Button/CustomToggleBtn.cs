using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CustomToggleBtn: BaseBtn
{
    public virtual void Select() { }
    public virtual void CancelSelect() { }
}
public class CustomToggleBtn<T> : CustomToggleBtn 
where T: CustomToggleBtn
{
    [LabelText("是否可以将自身关闭")]
    public bool isCloseItself;
    protected static T currentSelect;

    /// <summary>
    /// 选择
    /// </summary>
    public override void Select()
    {
        if (currentSelect == null)
        {
            currentSelect = this as T;
        }
        else
        {
            if (isCloseItself)
            {
                if (currentSelect == this as T)
                {
                    currentSelect.CancelSelect();
                    currentSelect = null;
                }
                else
                {
                    currentSelect.CancelSelect();
                    currentSelect = this as T;
                }
            }
            else
            {
                if (currentSelect != this)
                {
                    currentSelect.CancelSelect();
                    currentSelect = this as T;
                }
            }
        }
    }

    /// <summary>
    /// 取消选择
    /// </summary>
    public override void CancelSelect()
    {
        currentSelect = null;
    }
}
