using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class InteractionManager : BaseMonoIOCComponent, ISendEvent
{
    [SerializeField]
    private GameObject eventSystem;
    [SerializeField]
    private PhysicsRaycaster eaycaster3D;
    [SerializeField]
    private Physics2DRaycaster eaycaster2D;
    private int invokeCount;
    private bool interactionState = true;
    public bool InteractionState { get { return interactionState; } }


    /// <summary>
    /// 启用交互
    /// </summary>
    public void EnableInteraction()
    {
        invokeCount--;
        if (invokeCount == 0)
        {
            this.SendEvent(new ChangeInteractionState(true));
            interactionState = true;
            SetInteractionState(true);
        }
    }

    /// <summary>
    /// 禁止交互
    /// </summary>
    public void DisableInteraction()
    {
        invokeCount++;
        if (interactionState)
        {
            this.SendEvent(new ChangeInteractionState(false));
            interactionState = false;
            SetInteractionState(false);
        }
    }

    private void SetInteractionState(bool state)
    {
        eventSystem.SetActive(state);
        eaycaster3D.enabled = state;
        eaycaster2D.enabled = state;
    }
}
