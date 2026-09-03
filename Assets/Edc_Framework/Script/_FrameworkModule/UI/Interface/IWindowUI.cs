using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowUI
{
    /// <summary>
    /// 遮挡
    /// </summary>
    void Cover();

    /// <summary>
    /// 恢复
    /// </summary>
    void Reveal();
}
