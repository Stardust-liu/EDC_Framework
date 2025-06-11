using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RedDotLeafNodeBtn : MonoBehaviour, IPointerClickHandler, IAutoBindEvent
{
    public RedDotLeafNode redDotNode;
    public Image redDotIcon;
    protected bool IsActive{ get {return Hub.RedDotTree.CheckRedDotState(redDotNode); }}

    protected virtual void Start()
    {
        UpdateState();
        this.AddListener_StartDestroy<UpdateRedDotLeafNodeState>(UpdateRedDotLeafNodeState, gameObject);
    }

    private void UpdateRedDotLeafNodeState(UpdateRedDotLeafNodeState updateRedDotLeafNodeState){
        if(redDotNode == updateRedDotLeafNodeState.redDotLeafNode){
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Hub.RedDotTree.DisableRedDot(redDotNode);
    }
}
