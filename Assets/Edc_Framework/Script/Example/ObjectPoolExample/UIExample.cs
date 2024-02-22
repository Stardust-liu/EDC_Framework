using System.Collections;
using System.Collections.Generic;
using Example;
using UnityEngine;

public class UIExample : MonoBehaviour
{
    public CustomClickEffectsBtn ChangeView1Btn;
    public CustomClickEffectsBtn ChangeView2Btn;
    public CustomClickEffectsBtn openPersistentViewBtn;
    public CustomClickEffectsBtn OpenWindow1Btn;

    private void Start() {
        ChangeView1Btn.onClickEvent.AddListener(ClickChangeView1Btn);
        ChangeView2Btn.onClickEvent.AddListener(ClickChangeView2Btn);
        openPersistentViewBtn.onClickEvent.AddListener(ClickOpenPersistentViewBtn);
        OpenWindow1Btn.onClickEvent.AddListener(ClickOpenWindow1Btn);
    }

    private void ClickChangeView1Btn(){
        Hub.View.ChangeView(View1_C.Instance);
    }

    private void ClickChangeView2Btn(){
        Hub.View.ChangeView(View2_C.Instance);
    }

    private void ClickOpenPersistentViewBtn(){
        Hub.View.OpenPersistentView(View3_C.Instance);
    }

    private void ClickOpenWindow1Btn(){
        Hub.Window.OpenWindow(Window1_C.Instance);
    }
}
