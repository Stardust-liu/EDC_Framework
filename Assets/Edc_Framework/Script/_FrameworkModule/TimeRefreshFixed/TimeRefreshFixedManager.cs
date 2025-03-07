using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchiveData;

public class TimeRefreshFixedManager : BaseIOCComponent, IUpdate
{   
    private DateTime timeNow;
    private HashSet<string> needRemoveTimeKey;
    private HashSet<string> leftTimeManagerKey;
    private Dictionary<string, float> leftTimeManager;
    private TimeRefreshFixedData data;
    public static readonly EventCenter eventCenter = new EventCenter();

    protected override void Init()
    {
        timeNow = DateTime.Now;
        data = GameArchive.TimeRefreshFixedData;
        needRemoveTimeKey = new HashSet<string>();
        leftTimeManagerKey = new HashSet<string>();
        leftTimeManager = new Dictionary<string, float>();
        Hub.Update.AddUpdate(this);
        
        foreach (var item in data.timeManager.Keys)
        {
            var leftTime = InitGetLeftTime(item);
            if(leftTime >= 0f){
                leftTimeManager.Add(item, leftTime);
                leftTimeManagerKey.Add(item);
            }
            else{
                data.AddIdleOverflowTime(item, -leftTime);
                needRemoveTimeKey.Add(item);
            }
        }
        foreach (var item in needRemoveTimeKey)
        {
            data.RemoveTimeRecordId(item);
        }
        needRemoveTimeKey.Clear();
    }

    private int InitGetLeftTime(string timeRecordId){
        var waiTimeInfo = data.timeManager[timeRecordId];
        var leftTime = timeNow - waiTimeInfo.startTime;
        return waiTimeInfo.waitTime - (int)leftTime.TotalSeconds;
    }

    public void OnUpdate()
    {
        var unscaledDeltaTime = Time.unscaledDeltaTime;
        foreach (var item in leftTimeManagerKey)
        {
            leftTimeManager[item] -= unscaledDeltaTime;
            if(leftTimeManager[item] <= 0){
                needRemoveTimeKey.Add(item);
            }
        }
        if(needRemoveTimeKey.Count > 0){
            foreach (var item in needRemoveTimeKey)
            {
                data.RemoveTimeRecordId(item);
                leftTimeManager.Remove(item);
                leftTimeManagerKey.Remove(item);
                eventCenter.OnEvent(item);
            }
            needRemoveTimeKey.Clear();
        }    
    }


    /// <summary>
    /// 获取剩余刷新时间
    /// </summary>
    public float GetRemainTime(string timeRecordId){
        if(leftTimeManagerKey.Contains(timeRecordId)){
            return leftTimeManager[timeRecordId];
        }
        else{
            return -1;
        }
    }

    /// <summary>
    /// 获取挂机状态下超过的时间
    /// </summary>
    public float GetAfkExceededTime(string timeRecordId){
        return data.GetExceededTime(timeRecordId);
    }

    /// <summary>
    /// 获取挂机状态下刷新的次数及下次刷新需要的时间
    /// </summary>
    public void GetAfkRefreshInfo(string timeRecordId, int waitSecond, out int TimeoutCount, out int nextRefreshNeedTime){
        var exceededTime = GetAfkExceededTime(timeRecordId);
        TimeoutCount =(int)(exceededTime / waitSecond);
        nextRefreshNeedTime = (int)exceededTime % waitSecond;
    }
    
    /// <summary>
    /// 添加时间记录ID（如果已经有这个ID，会直接对这个Key对应的Value进行更新）
    /// </summary>
    public void AddTimeRecordId(string timeRecordId , int waitSecond){
        data.AddTimeRecordId(timeRecordId, waitSecond, timeNow.AddSeconds(Time.time));
        if(leftTimeManager.ContainsKey(timeRecordId)){
            leftTimeManager[timeRecordId] = waitSecond;
        }
        else{
            leftTimeManager.Add(timeRecordId, waitSecond);
            leftTimeManagerKey.Add(timeRecordId);
        }
    }


    /// <summary>
    /// 中止时间记录
    /// </summary>
    public void RemoveTimeRecordId(string timeRecordId){
        if(leftTimeManagerKey.Contains(timeRecordId)){
            data.RemoveTimeRecordId(timeRecordId);
            leftTimeManager.Remove(timeRecordId);
            leftTimeManagerKey.Remove(timeRecordId);
        }
    }
}
