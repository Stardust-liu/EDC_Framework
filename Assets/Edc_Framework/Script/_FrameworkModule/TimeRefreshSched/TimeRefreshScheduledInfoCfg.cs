using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TimeRefreshScheduledInfoVO{
    public string periodicID;
    public int waitDays;
    public int refreshHours; 
}

public class TimeRefreshScheduledInfoCfg : ParsCsv<TimeRefreshScheduledInfoCfg>
{
    protected static Dictionary<string, TimeRefreshScheduledInfoVO> scheduledRefreshInfo = new();
    public ReadOnlyDictionary<string, TimeRefreshScheduledInfoVO> ScheduledRefreshInfo = new(scheduledRefreshInfo);

    public TimeRefreshScheduledInfoCfg() : base(){
        ParseData(Resources.Load<TextAsset>("CSV/TimeRefreshScheduled/TimeRefreshScheduledInfo"));
    }

    protected override void SetData()
    {
        var vo = new TimeRefreshScheduledInfoVO();
        vo.periodicID = GetString("PeriodicID");
        vo.waitDays = GetInt("WaitDays");
        vo.refreshHours = GetInt("RefreshHours");
        scheduledRefreshInfo.Add(vo.periodicID, vo);
    }

    public int GetRefreshDays(string PeriodicID){
        return scheduledRefreshInfo[PeriodicID].waitDays;
    }
}
