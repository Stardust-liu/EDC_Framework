using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RedDotExample : MonoBehaviour
{
    public CustomClickEffectsBtn resetRedDotBtn;

    void Awake() {
        if(FrameworkManager.isInitFinish){
            ResetRedDot();
        }
    }

    void Start()
    {
        resetRedDotBtn.onClickEvent.AddListener(ResetRedDot);
    }

    /// <summary>
    /// 重置红点状态
    /// </summary>
    public void ResetRedDot(){
        foreach (RedDotLeafNode item in Enum.GetValues(typeof(RedDotLeafNode)))
        {
            Hub.RedDotTree.ActiveRedDot(item);
        }
    }
}
