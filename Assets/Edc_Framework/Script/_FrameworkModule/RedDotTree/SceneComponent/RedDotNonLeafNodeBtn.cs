using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDotNonLeafNodeBtn : MonoBehaviour, IAutoBindEvent
{
    public RedDotNode redDotNode;
    public Image redDotIcon;
    protected bool IsActive{ get {return Hub.RedDotTree.CheckRedDotState(redDotNode); }}

    protected virtual void Start()
    {
        UpdateState();
        this.AddListener_StartDestroy<UpdateRedDotNodeState>(UpdateRedDotNodeState, gameObject);
    }

    private void UpdateRedDotNodeState(UpdateRedDotNodeState updateRedDotNodeState){
        if(redDotNode == updateRedDotNodeState.redDotNode){
            UpdateState();
        }
    }
    protected virtual void UpdateState(){
        if(IsActive){
            ShowRedDot();
        }
        else{
            HideRedDot();
        }
    }

    protected virtual void ShowRedDot(){
        redDotIcon.enabled = true;
    }

    protected virtual void HideRedDot(){
        redDotIcon.enabled = false;
    }
}
