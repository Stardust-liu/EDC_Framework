using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnSound_Enter : BasePlaySoundEffect, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound();
    }
}
