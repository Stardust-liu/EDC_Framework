using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



public class SelectableAnimEffect : BaseSelectableEffect
{
    public Animator[] animator;
    protected override void ChangeState()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        foreach (var item in animator)
        {
            item.SetTrigger(CurrentState.ToString());
        }
    }
}
