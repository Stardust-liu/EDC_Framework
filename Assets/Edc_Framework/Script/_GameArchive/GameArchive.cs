using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    public static class GameArchive
    {
        #region 框架数据
        public static AudioData AudioData{get; private set;}
        public static AchievementData AchievementData{get; private set;}
        public static LanguageData LanguageData{get; private set;}
        public static RedDotData RedDotData{get; private set;}
        public static TimeRefreshFixedData TimeRefreshFixedData{get; private set;}
        public static TimeRefreshScheduledData TimeRefreshScheduledData{get; private set;}
        public static UIControllerData UIControllerData{get; private set;}
        #endregion

        #region 游戏数据 
        #endregion

        /// <summary>
        /// 初始化数据
        /// </summary>
        public static void Init(){
            AudioData = AudioData.ReadData();
            AchievementData = AchievementData.ReadData();
            LanguageData = LanguageData.ReadData();
            RedDotData = RedDotData.ReadData();
            TimeRefreshFixedData = TimeRefreshFixedData.ReadData();
            TimeRefreshScheduledData = TimeRefreshScheduledData.ReadData();
            UIControllerData = UIControllerData.ReadData();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public static void ClearData(){
            ((IGameArchive)AudioData).CleanData();
            ((IGameArchive)AchievementData).CleanData();
            ((IGameArchive)LanguageData).CleanData();
            ((IGameArchive)RedDotData).CleanData();
            ((IGameArchive)TimeRefreshFixedData).CleanData();
            ((IGameArchive)TimeRefreshScheduledData).CleanData();
            ((IGameArchive)UIControllerData).CleanData();
        }
    }
}
