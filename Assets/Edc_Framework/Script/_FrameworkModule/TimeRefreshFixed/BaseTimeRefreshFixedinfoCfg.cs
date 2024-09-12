using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using UnityEngine;

public class TimeRefreshFixedInfoVO{
    public string timeRecordId;
    public int waitSecond;
}

public class BaseTimeRefreshFixedInfoCfg<T> : ParsCsv<T> where T : class, new()
{
    private static Dictionary<string, TimeRefreshFixedInfoVO> fixedRefreshInfo = new();
    public ReadOnlyDictionary<string, TimeRefreshFixedInfoVO> FixedRefreshInfo = new(fixedRefreshInfo);
    protected override void SetData()
    {
        var key = GetString("ID");
        var timeRefreshFixedInfoVO = new TimeRefreshFixedInfoVO();
        timeRefreshFixedInfoVO.timeRecordId = key;
        timeRefreshFixedInfoVO.waitSecond = GetInt("WaitSecond");
        fixedRefreshInfo.Add(key, timeRefreshFixedInfoVO);
    }
}
