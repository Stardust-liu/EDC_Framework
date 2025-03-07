using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : BaseMonoIOCComponent
{
    //启动协程
    public new Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return base.StartCoroutine(coroutine);
    }

    // 停止协程
    public new void StopCoroutine(Coroutine coroutine)
    {
        base.StopCoroutine(coroutine);
    }
}
