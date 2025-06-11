using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExample : MonoBehaviour
{
    void Start()
    {
        Hub.View.ChangeView<ViewExample_A>();
    }
}
