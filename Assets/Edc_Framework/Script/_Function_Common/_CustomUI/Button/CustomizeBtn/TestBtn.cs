using System.Collections;
using System.Collections.Generic;
using CustomizeUI;
using UnityEngine;

public class TestBtn : MonoBehaviour
{
    public CustomizeBtn customizeBtn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            //customizeBtn.Aaaaaa = true;
        }
        if(Input.GetKeyDown(KeyCode.D)){
            //customizeBtn.Aaaaaa = false;
        }
    }
}
