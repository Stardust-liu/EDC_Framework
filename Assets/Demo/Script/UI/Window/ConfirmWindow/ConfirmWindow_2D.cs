using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow_2D : BaseWindow
{
    public Button confirmBtn;
    public Button cancelBtn;
    public LocalizationText text;
    private Action confirmAction;

    protected override void Init()
    {
        base.Init();
        confirmBtn.onClick.AddListener(ClickConfirmBtn);
        cancelBtn.onClick.AddListener(ClickCancelBtn);
    }

    protected override void StartShow()
    {
        base.StartShow();
        ShowFinish();
    }

    protected override void StartHide()
    {
        base.StartHide();
        HideFinish();
    }

    public void InitInfo(string key, Action action)
    {
        text.RefreshContent(key);
        confirmAction = action;
    }

    private void ClickConfirmBtn()
    {
        confirmAction.Invoke();
    }

    private void ClickCancelBtn()
    {
        Hub.Window.CloseWindow();
    }
}
