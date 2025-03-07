using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    private float displaySpeed = -1;
    [SerializeField]
    private TextMeshProUGUI contentText;
    private WaitForSeconds waitForSeconds;
    private IEnumerator typeWriterEffect;
    private readonly StringBuilder contentData = new();

    /// <summary>
    /// 开始输出文字
    /// </summary>
    public void StartTypeWriterEffect(string content, float _displaySpeed, Action callBack = null){
        if(displaySpeed != _displaySpeed){
            displaySpeed = _displaySpeed;
            waitForSeconds = new WaitForSeconds(displaySpeed);
        }
        typeWriterEffect = TypeWriterEffect(content, waitForSeconds, callBack);;
        StartCoroutine(typeWriterEffect);
    }

    /// <summary>
    /// 停止输出文字
    /// </summary>
    public void StopTypeWriterEffect(Action callBack = null){
        if(typeWriterEffect != null){
            StopCoroutine(typeWriterEffect);
            callBack?.Invoke();
            typeWriterEffect = null;
        }
    }

    /// <summary>
    /// 显示所有文字内容
    /// </summary>
    public void DisplayAllContent(string _content){
        contentText.text = _content;
        if(typeWriterEffect != null){
            typeWriterEffect = null;
        }
    }

    /// <summary>
    /// 是否正在进行文字输出
    /// </summary>
    public bool IsTypeWriterEffectRunning(){
        return typeWriterEffect != null;
    }


   /// <summary>
   /// 文字输出效果
   /// </summary>
    private IEnumerator TypeWriterEffect(string content, WaitForSeconds displaySpeed, Action callBack = null){
        int showIndex = 0;
        int endIndex = content.Length-1;
        contentData.Clear();
        contentData.Capacity = content.Length;
        contentData.Append(content[0]);
        contentText.text = contentData.ToString();
        while(showIndex < endIndex){
            yield return displaySpeed;
            showIndex++;
            contentData.Append(content[showIndex]);
            contentText.text = contentData.ToString();
        }
        callBack?.Invoke();
        typeWriterEffect = null;
    }
}
