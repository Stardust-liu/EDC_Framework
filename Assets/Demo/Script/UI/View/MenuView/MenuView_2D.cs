using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class MenuView_2D : BaseDescView
{
    public OptionItemBtn freeMissionBtn;
    public OptionItemBtn dataViewerBtn;
    public OptionItemBtn optionsBtn;
    public OptionItemBtn quitGameBtn;

    protected override void Init()
    {
        base.Init();
        freeMissionBtn.SetInitAction(ClickFreeMissionBtnBtn, ChangeOptionItem, ExitOptionItem);
        dataViewerBtn.SetInitAction(ClickDataViewerBtn, ChangeOptionItem, ExitOptionItem);
        optionsBtn.SetInitAction(ClickOptionsBtn, ChangeOptionItem, ExitOptionItem);
        quitGameBtn.SetInitAction(ClickQuitGameBtn, ChangeOptionItem, ExitOptionItem);
    }

    private void ClickFreeMissionBtnBtn()
    {
        ConfirmSelect();
        Hub.ScreenTransition.FadeIn(Color.black, WaitTime.mediumSpeed).SetDelay(WaitTime.slow);
        Hub.View.ChangeView<FreeMissionView_C>(ChangeFinish);
        void ChangeFinish()
        {
            Hub.ScreenTransition.FadeOut(WaitTime.mediumSpeed);
        }
    }

    public void ClickDataViewerBtn()
    {
        ConfirmSelect();
    }

    private void ClickOptionsBtn()
    {
        ConfirmSelect();
        Hub.ScreenTransition.FadeIn(Color.black, WaitTime.mediumSpeed).SetDelay(WaitTime.slow);
        Hub.View.ChangeView<SettingView_C>(ChangeFinish);
        void ChangeFinish()
        {
            Hub.ScreenTransition.FadeOut(WaitTime.mediumSpeed);
        }
    }

    private void ClickQuitGameBtn()
    {
        ConfirmSelect(OpenQuitGameWindow);
    }

    private void OpenQuitGameWindow()
    {
        var window = Hub.Window;
        window.OpenWindow<ConfirmWindow_C>(InitConfirmWindow);
        void InitConfirmWindow(ConfirmWindow_C confirmWindow)
        {
            confirmWindow.InitInfo("ConfirmWindow_Quit_Desc", Application.Quit);
        }
    }
}
