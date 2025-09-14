using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSound_Click : BasePlaySoundEffect
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(PlaySound);
    }
}
