using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnterSceneControl : MonoBehaviour
{
    private void Start()
    {
        Init();   
    }
    public abstract void Init();
}
