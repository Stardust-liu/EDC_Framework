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
    private Dictionary<BtnClickAudioType, ResourcePath> audioInfo;
    protected override void InitData()
    {
        audioInfo = new(RowCount);
    }

    protected override void SetData()
    {
        //audioInfo.Add(GetEnum<BtnClickAudioType>("audioID"), GetString("audioPath"));
    }

    /// <summary>
    /// 获取音频路径
    /// </summary>
    public ResourcePath GetPath(BtnClickAudioType btnClickAudioType){
        return audioInfo[btnClickAudioType];
    }
}
