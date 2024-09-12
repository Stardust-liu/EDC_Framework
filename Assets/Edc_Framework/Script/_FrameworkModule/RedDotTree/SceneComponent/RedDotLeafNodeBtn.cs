using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RedDotLeafNodeBtn : MonoBehaviour, IPointerClickHandler
{
    public RedDotLeafNode redDotNode;
    public Image redDotIcon;
    protected bool IsActive{ get {return Hub.RedDotTree.CheckRedDotState(redDotNode); }}
    private static readonly EventCenter eventCenter = RedDotTreeManager.eventCenter;

    protected virtual void Start()
    {
        UpdateState();
        eventCenter.AddListener(redDotNode.ToString(), UpdateState);
    }

    protected virtual void OnDestroy() {
        eventCenter.RemoveListener(redDotNode.ToString(), UpdateState);
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
