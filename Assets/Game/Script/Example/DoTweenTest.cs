using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DoTweenTest : MonoBehaviour
{
    public Transform test1;
    public Transform test2;
    private Sequence a;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            test1.DOMoveX(5,5).OnComplete(()=>{Debug.Log("1");});
        }
        if(Input.GetKeyDown(KeyCode.E)){
            //test1.DOKill();
            test1.DOMoveX(5,10).OnComplete(()=>{Debug.Log("2");});
        }
    }
}
