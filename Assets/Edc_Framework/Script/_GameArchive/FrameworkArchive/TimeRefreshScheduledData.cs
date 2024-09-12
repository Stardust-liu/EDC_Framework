using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArchiveData{
    public class ScheduledRefreshInfo{
        public DateTime startTime;
        public DateTime nextRefreshTime;

        public ScheduledRefreshInfo(DateTime _startTime, DateTime _nextRefreshTime){
            startTime = _startTime;
            nextRefreshTime = _nextRefreshTime;
        }
    }
    public class TimeRefreshScheduledData : BaseGameArchive<TimeRefreshScheduledData>
    {
        public Dictionary<string, ScheduledRefreshInfo> refreshTime = new ();
        public Dictionary<string, HashSet<string>> refreshItemMark = new ();

        /// <summary>
        /// 添加刷新时间
        /// </summary>
        public void AddRefreshTime(string periodicID, DateTime startTime, DateTime nextRefreshTime){
            refreshTime.Add(periodicID, new ScheduledRefreshInfo(startTime, nextRefreshTime));
            SaveDataNow();
        }

        /// <summary>
        /// 移除刷新时间
        /// </summary>
        public void RemoveRefreshTime(string periodicID){
            if(refreshTime.ContainsKey(periodicID)){
                refreshTime.Remove(periodicID);
                SaveDataNow();
            }
            else{
                LogManager.LogError($"字典没有这个计划更新ID：{periodicID}");
            }
        }

        /// <summary>
        /// 更新下次刷新时间
        /// </summary>
        public void UpdateNextRefreshTime(string periodicID, DateTime startTime, DateTime nextRefreshTime){
            refreshTime[periodicID].startTime = startTime;
            refreshTime[periodicID].nextRefreshTime = nextRefreshTime;
            SaveDataNow();
        }

        /// <summary>
        /// 修改下次刷新时间
        /// </summary>
        public void ChangeNextRefreshTime(string periodicID, DateTime time){
            refreshTime[periodicID].nextRefreshTime = time;
            SaveDataNow();
        }

        /// <summary>
        /// 添加刷新记录
        /// </summary>
        public void AddRefreshItemMark(string periodicID, string itemID){
            if(!refreshItemMark.ContainsKey(periodicID)){
                refreshItemMark.Add(periodicID, new HashSet<string>());
            }
            refreshItemMark[periodicID].Add(itemID);
            SaveDataNow();
        }

        /// <summary>
        /// 清除已刷新信息
        /// </summary>
        public void CleanRefreshItemMark(string periodicID){
            refreshItemMark.Clear();
            SaveDataNow();
        }

        /// <summary>
        /// 是否可以刷新
        /// </summary>
        public bool IsCanRefresh(string periodicID, string itemID){
            if(refreshItemMark.ContainsKey(periodicID)){
                return !refreshItemMark[periodicID].Contains(itemID);
            }
            else{
                return true;
            }
        }
    }
}
