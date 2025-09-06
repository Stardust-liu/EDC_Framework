using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseSelectItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform selectEffectParent;

    protected abstract void ChangeOptionItem();
    protected abstract void ExitBtn();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Hub.Interaction.InteractionState)
        {
            ChangeOptionItem();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Hub.Interaction.InteractionState)
        {
            ExitBtn();
        }
    }
}
