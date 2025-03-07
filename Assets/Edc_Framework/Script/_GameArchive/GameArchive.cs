using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{

    public class GameArchive : IOCContainer<GameArchive>
    {
        //需要在整个游戏声明周期内都使用的数据使用对象的方式声明
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

        protected override void InitContainer()
        {
            AudioData = ((IContainer)this).Register(AudioData.ReadData());
            AchievementData = ((IContainer)this).Register(AchievementData.ReadData());
            LanguageData = ((IContainer)this).Register(LanguageData.ReadData());
            RedDotData = ((IContainer)this).Register(RedDotData.ReadData());
            TimeRefreshFixedData = ((IContainer)this).Register(TimeRefreshFixedData.ReadData());
            TimeRefreshScheduledData = ((IContainer)this).Register(TimeRefreshScheduledData.ReadData());
            UIControllerData = ((IContainer)this).Register(UIControllerData.ReadData());
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
