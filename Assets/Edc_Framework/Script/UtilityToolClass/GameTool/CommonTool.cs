using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CommonTool
{
    
#region 打字机效果
    /// <summary>
    /// 打字机效果
    /// </summary>
    public static void TypeWriterEffect(TextMeshProUGUI tmpGui, string content, float displaySpeed, Action callBack = null){
        TypeWriterEffect(tmpGui, content, new WaitForSeconds(displaySpeed), callBack);
    }

   /// <summary>
   /// 打字机效果
   /// </summary>
    public static IEnumerator TypeWriterEffect(TextMeshProUGUI tmpGui, string content, WaitForSeconds displaySpeed, Action callBack = null){
        var typeWriterEffect = TypeWriterEffect();
        FrameworkManager.instance.StartCoroutine(typeWriterEffect);
        IEnumerator TypeWriterEffect(){
            int showIndex = 0;
            int endIndex = content.Length-1;
            var textData = new StringBuilder(content.Length);
            textData.Append(content[0]);
            tmpGui.text = textData.ToString();
            while(showIndex < endIndex){
                yield return displaySpeed;
                showIndex++;
                textData.Append(content[showIndex]);
                tmpGui.text = textData.ToString();
            }
            callBack?.Invoke();
        }
        return typeWriterEffect;
    }

    /// <summary>
    /// 停止打字机效果
    /// </summary>
    public static void StopTypeWriterEffect(IEnumerator typeWriterEffect, Action callBack = null){
        FrameworkManager.instance.StopCoroutine(typeWriterEffect);
        callBack?.Invoke();
    }
#endregion
}
