using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RedDotExample : MonoBehaviour
{
    public Button resetRedDotBtn;

    void Awake() {
        if(FrameworkManager.isInitFinish){
            ResetRedDot();
        }
    }

    void Start()
    {
        resetRedDotBtn.onClick.AddListener(ResetRedDot);
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
