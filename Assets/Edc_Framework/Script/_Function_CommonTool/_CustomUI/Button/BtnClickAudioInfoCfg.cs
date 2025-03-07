using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BtnClickAudioType{
    None,
    General,
    Confirm,
    Cancel
}

public class BtnClickAudioInfoCfg : ParsCsv<BtnClickAudioInfoCfg>
{
    private readonly Dictionary<BtnClickAudioType, string> audioInfo = new();
    protected override void SetData()
    {
        audioInfo.Add(GetEnum<BtnClickAudioType>("audioID"), GetString("audioPath"));
    }

    /// <summary>
    /// 获取音频路径
    /// </summary>
    public string GetPath(BtnClickAudioType btnClickAudioType){
        return audioInfo[btnClickAudioType];
    }
}
