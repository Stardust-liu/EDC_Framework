using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExample : MonoBehaviour
{
    public Slider setHorizontalMargin;
    void Start()
    {

        setHorizontalMargin.onValueChanged.AddListener(ChangeHorizontalMargin);
        Hub.View.ChangeView<TestView_C>();
    }

    private void ChangeHorizontalMargin(float value)
    {
        Hub.UI.SetHorizontalMargin(value);
    }
}
