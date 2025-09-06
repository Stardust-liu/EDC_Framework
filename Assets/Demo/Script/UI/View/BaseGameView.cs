using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseReturnableView : BaseDescView
{
    public OptionItemBtn bcak;
    private BaseReturnableViewLogic logic;
    protected override void Init()
    {
        base.Init();
        logic = new();
        bcak.SetInitAction(Back, ChangeOptionItem, ExitOptionItem);
    }

    private void Back()
    {
        ConfirmSelect();
        logic.Bcak();
    }
}

public class BaseReturnableView<Model> : BaseDescView<Model> where Model : BaseUI_Model, new()
{
    public OptionItemBtn bcak;
    private BaseReturnableViewLogic logic;
    protected override void Init()
    {
        base.Init();
        logic = new();
        bcak.SetInitAction(Back, ChangeOptionItem, ExitOptionItem);
    }

    private void Back()
    {
        ConfirmSelect();
        logic.Bcak();
    }
}

public class BaseReturnableViewLogic {
    public void Bcak()
    {
        Hub.ScreenTransition.FadeIn(Color.black, WaitTime.mediumSpeed).SetDelay(WaitTime.slow);
        Hub.View.BackLastView(BackLastViewFinish);
        void BackLastViewFinish()
        {
            Hub.ScreenTransition.FadeOut(WaitTime.mediumSpeed);
        }
    }
}

public class BaseDescView : BaseView
{
    [PropertyOrder(1)]
    public LocalizationText[] localizationText;
    public SelectEffect selectEffect;
    private BaseDescViewLogic logic;

    protected override void Init()
    {
        base.Init();
        logic = new BaseDescViewLogic(selectEffect, localizationText);
        logic.CleanTextContent();
    }

    protected void ChangeOptionItem(Transform parent, string[] descID)
    {
        logic.ChangeOptionItem(parent, descID);
    }

    protected void ExitOptionItem()
    {
        logic.ExitOptionItem();
    }

    protected void ConfirmSelect(Action _confirmSelectComplete = null)
    {
        logic.ConfirmSelect(_confirmSelectComplete);
    } 
}

public class BaseDescView<Model> : BaseView<Model> where Model : BaseUI_Model, new()
{
    public SelectEffect selectEffect;
    public LocalizationText[] localizationText;
    private BaseDescViewLogic logic;

    protected override void Init()
    {
        base.Init();
        logic = new BaseDescViewLogic(selectEffect, localizationText);
        logic.CleanTextContent();
    }

    protected virtual void ChangeOptionItem(Transform parent, string[] descID)
    {
        logic.ChangeOptionItem(parent, descID);
    }

    protected virtual void ExitOptionItem()
    {
        logic.ExitOptionItem();
    }

    protected void ConfirmSelect(Action _confirmSelectComplete = null)
    {
        logic.ConfirmSelect(_confirmSelectComplete);
    }
}

public class BaseDescViewLogic
{
    private SelectEffect selectEffect;
    private LocalizationText[] localizationText;

    public BaseDescViewLogic(SelectEffect _selectEffect, LocalizationText[] _localizationText)
    {
        selectEffect = _selectEffect;
        localizationText = _localizationText;
    }


    public void ChangeOptionItem(Transform parent, string[] descID)
    {
        selectEffect.ChangeOptionItem(parent);
        if (descID != null)
        {
            var count = localizationText.Length;
            var descCount = descID.Length;
            for (var i = 0; i < count; i++)
            {
                if (i < descCount)
                {
                    localizationText[i].RefreshContent(descID[i], true);
                }
                else
                {
                    localizationText[i].CleanTextContent();
                }
            }   
        }
    }

    public void ConfirmSelect(Action _confirmSelectComplete)
    {
        selectEffect.ConfirmSelect(_confirmSelectComplete);
        CleanTextContent();
    }

    public void ExitOptionItem()
    {
        selectEffect.Hide();
        CleanTextContent();
    }

    public void CleanTextContent()
    {
        foreach (var item in localizationText)
        {
            item.CleanTextContent();
        }
    }
}
