using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{

    public class FixedRefreshInfo{
        public DateTime startTime;
        public int waitTime;

        public FixedRefreshInfo(DateTime _startTime, int _waitTime){
            startTime = _startTime;
            waitTime = _waitTime;
        }
    }
    public class TimeRefreshFixedData : BaseGameArchive<TimeRefreshFixedData>
    {
        public Dictionary<string, int> idleOverflowTime = new();
        public Dictionary<string, FixedRefreshInfo> timeManager = new();

        /// <summary>
        /// 添加时间记录
        /// </summary>
        public void AddTimeRecordId(string timeRecordId , int waitSecond, DateTime startTime){
            timeManager.Add(timeRecordId, new FixedRefreshInfo(startTime, waitSecond));
            if(idleOverflowTime.ContainsKey(timeRecordId)){
                idleOverflowTime.Remove(timeRecordId);
            }
            SaveDataNow();
        }

        /// <summary>
        /// 添加挂机超出时间
        /// </summary>
        public void AddIdleOverflowTime(string timeRecordId, int leftTime){
            idleOverflowTime.Add(timeRecordId, leftTime);
            SaveDataNow();
        }

        /// <summary>
        /// 移除时间记录
        /// </summary>
        public void RemoveTimeRecordId(string timeRecordId){
            if(timeManager.ContainsKey(timeRecordId)){
                timeManager.Remove(timeRecordId);
                SaveDataNow();
            }
            else{
                LogManager.LogError($"没有这个时间记录ID：{timeRecordId}");
            }
        }

        /// <summary>
        /// 获取挂机状态下超过的时间
        /// </summary>
        public float GetExceededTime(string timeRecordId){
            if(data.idleOverflowTime.ContainsKey(timeRecordId)){
                return data.idleOverflowTime[timeRecordId];
            }
            else{
                return 0;
            }
        }
    }
}
