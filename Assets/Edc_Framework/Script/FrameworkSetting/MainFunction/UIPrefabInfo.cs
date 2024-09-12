using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIPrefabInfo
{
    public GameObject prefab;
    [LabelText("在关闭完成后销毁对象")]
    public bool isHideFinishDestroy;
}
