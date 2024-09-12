using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Sirenix.OdinInspector;
using UnityEditor;
public abstract class RedDotTreeBaseNode : BaseNode 
{
    //[ReadOnly]
    public bool isActive;

    /// <summary>
    /// 激活红点
    /// </summary>
    public virtual void ActiveRedDot(){
        isActive = true;
    }

    /// <summary>
    /// 禁用红点
    /// </summary>
    public virtual void DisableRedDot(){
        isActive = false;
    }
}
