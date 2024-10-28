using System.Collections;
using System.Collections.Generic;
using Example;
using UnityEngine;
using UnityEngine.UI;

public class UIExample : MonoBehaviour
{
    private void Start(){
        Hub.View.ChangeView(View1_P.Instance);
    }
}
