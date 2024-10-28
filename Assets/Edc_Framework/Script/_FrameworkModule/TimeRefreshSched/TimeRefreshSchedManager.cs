using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchiveData;
public class TimeRefreshSchedEventName{
    public const string updateTimeRefreshSched = nameof(updateTimeRefreshSched);
}

public class TimeRefreshSchedManager
{
    private readonly TimeRefreshScheduledInfoCfg timeRefreshScheduledInfoCfg;
    private readonly Dictionary<string, ScheduledRefreshInfo> refreshTime;
    private TimeRefreshScheduledData data;
    public static readonly EventCenter eventCenter = new EventCenter();

    public TimeRefreshSchedManager(){
        data = GameArchive.TimeRefreshScheduledData;
        timeRefreshScheduledInfoCfg = TimeRefreshScheduledInfoCfg.Instance;
        refreshTime = data.refreshTime;
        var currentTime = DateTime.Now;
        var scheduledRefreshInfo = timeRefreshScheduledInfoCfg.ScheduledRefreshInfo;
        var today = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
        foreach (var item in scheduledRefreshInfo)
        {
            if(refreshTime.ContainsKey(item.Key)){
                CheckIsNeedRefresh(item.Key);
            }
            else{
                var days = item.Value.waitDays;
                var hours = item.Value.refreshHours;
                var startTime = today.AddHours(hours);
                var nextRefreshTime = today.AddDays(days).AddHours(hours);
                data.AddRefreshTime(item.Key, startTime, nextRefreshTime);
            }
        }
    }

    /// <summary>
    /// 整点更新
    /// </summary>
    public void UpdateHours(){
        CheckIsNeedRefresh();
    }

    /// <summary>
    /// 是否可以刷新
    /// </summary>
    public bool IsCanRefresh(string periodicID, string itemID){
        return data.IsCanRefresh(periodicID, itemID);
    }

    /// <summary>
    /// 获取详细刷新信息
    /// </summary>
    public TimeSpan GetDetailedRefreshTime(string PeriodicID){
        var currentTime = DateTime.Now;
        var specifyPeriodic = refreshTime[PeriodicID].nextRefreshTime;
        var timeDifference = specifyPeriodic - currentTime;
        return timeDifference;
    }

    /// <summary>
    /// 添加刷新记录
    /// </summary>
    public void AddRefreshItemMark(string periodicID, string itemID){
        data.AddRefreshItemMark(periodicID, itemID);
    }

    /// <summary>
    /// 检查是否需要刷新
    /// </summary>
    private void CheckIsNeedRefresh(){
        foreach (var item in refreshTime.Keys)
        {
            CheckIsNeedRefresh(item);
        }     
    }

    /// <summary>
    /// 修改下次刷新时间
    /// </summary>
    private void ChangeNextRefreshTime(string periodicID, int days){
        ChangeNextRefreshTime(periodicID, days, refreshTime[periodicID].startTime.Hour);
    }

    /// <summary>
    /// 修改下次刷新时间
    /// </summary>
    private void ChangeNextRefreshTime(string periodicID, int days, int hour){
        var nextRefreshTime = refreshTime[periodicID].startTime;
        nextRefreshTime.AddDays(days).AddHours(hour);
        data.ChangeNextRefreshTime(periodicID, nextRefreshTime);
    }   

    /// <summary>
    /// 检查是否需要刷新
    /// </summary>
    private void CheckIsNeedRefresh(string periodicID){
        if(DateTime.Compare(DateTime.Now, refreshTime[periodicID].nextRefreshTime) >= 0){
            SetStartTimeAndNextRefreshTime(periodicID, out DateTime startTime, out DateTime nextRefreshTime);
            data.UpdateNextRefreshTime(periodicID, startTime, nextRefreshTime);
            data.CleanRefreshItemMark(periodicID);
        }
        eventCenter.OnEvent(TimeRefreshSchedEventName.updateTimeRefreshSched);    
    }

    /// <summary>
    /// 设置起始时间与刷新时间
    /// </summary>
    private void SetStartTimeAndNextRefreshTime(string periodicID, out DateTime startTime, out DateTime nextRefreshTime){
        var lastNextRefreshTime = refreshTime[periodicID].nextRefreshTime;
        var days = timeRefreshScheduledInfoCfg.GetRefreshDays(periodicID);
        var exceededDays = (DateTime.Now - lastNextRefreshTime).Days;
        var missedRefreshCount = exceededDays / days;
        if(missedRefreshCount == 0){
            nextRefreshTime = lastNextRefreshTime.AddDays(days);
            startTime = lastNextRefreshTime;
        }
        else{
            nextRefreshTime = lastNextRefreshTime.AddDays(days * (missedRefreshCount+1));
            startTime = lastNextRefreshTime.AddDays(days * missedRefreshCount);
        }
    }
}
