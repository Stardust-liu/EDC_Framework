using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel
{
    public static Image loadPanel;
    private static Color baseColor = new Color(1, 1, 1, 0);

    public LoadPanel(Image loadPanel){
        LoadPanel.loadPanel = loadPanel;
    }

    public void Show(Color color){
        loadPanel.enabled = true;
        loadPanel.color = color;
    }

    public void FadeIn(Color color){
        loadPanel.enabled = true;

        loadPanel.color = color * baseColor;
        loadPanel.DOFade(1, WaitTime.viewGradientTime);
    }

    public void FadeOut(){
        if(loadPanel.enabled){
            loadPanel.DOFade(0, WaitTime.viewGradientTime).OnComplete(HideFinish);
        }
    }

    private void HideFinish(){
        loadPanel.enabled = false;
    }
}
