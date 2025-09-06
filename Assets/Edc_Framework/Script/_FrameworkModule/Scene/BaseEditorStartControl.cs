using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEditorStartControl : MonoBehaviour
{
#if UNITY_EDITOR
    public abstract void Init();
#endif
}
