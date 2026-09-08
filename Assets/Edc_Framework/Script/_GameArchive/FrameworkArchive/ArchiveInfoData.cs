using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ArchiveData
{
    /// <summary>
    /// 存档槽显示信息。
    /// 用于存档列表、继续游戏按钮等界面展示，不保存具体游戏进度
    /// </summary>
    [Serializable]
    public class ArchiveSlotInfo
    {
        /// <summary>
        /// 存档槽的唯一标识，同时也是槽位数据目录名
        /// </summary>
        public string slotKey;

        /// <summary>
        /// 存档槽在界面上显示的名字
        /// </summary>
        public string displayName;

        /// <summary>
        /// 当前槽位是否已经产生过存档数据
        /// </summary>
        public bool hasArchive;

        /// <summary>
        /// 保存时间，使用Unix时间戳记录
        /// </summary>
        public long SaveTime;

        [JsonConstructor]
        public ArchiveSlotInfo(string slotKey)
        {
            this.slotKey = slotKey;
        }

        public ArchiveSlotInfo(string slotKey, string displayName)
        {
            this.slotKey = slotKey;
            this.displayName = displayName;
        }
    }

    /// <summary>
    /// 存档系统的全局信息
    /// 只负责记录当前槽位和槽位列表，不保存具体游戏业务数据
    /// </summary>
    [ArchiveKey("ArchiveInfo", ArchiveDomain.Global)]
    public class ArchiveInfoData : BaseGameArchive
    {
        [JsonIgnore]
        public int SlotCount { get; private set; }

        [JsonIgnore]
        public string CurrentSlotKey => GetSlotInfo(currentSlotIndex).slotKey;
        
        /// <summary>
        /// 当前使用的存档槽索引
        /// </summary>
        public int currentSlotIndex;

        /// <summary>
        /// 存档槽的显示信息
        /// </summary>
        public List<ArchiveSlotInfo> slots;

        protected internal override void OnCreateDefaultData()
        {
            SlotCount = Mathf.Max(1, FrameworkManager.ArchiveSlotCount);
            slots ??= new List<ArchiveSlotInfo>(SlotCount);
            currentSlotIndex = 0;
            EnsureSlotCount();
        }

    
        protected internal override void OnAfterLoad()
        {
            SlotCount = Mathf.Max(1, FrameworkManager.ArchiveSlotCount);
            if (SlotCount != slots.Count)
            {
                EnsureSlotCount();
            }
        }

        /// <summary>
        /// 同步槽位列表数量
        /// </summary>
        public void EnsureSlotCount()
        {
            if (SlotCount < slots.Count)
            {
                LogManager.LogWarning($"存档槽配置数量从 {slots.Count} 减少到 {SlotCount}，多余的槽位显示信息会被移除");
                slots.RemoveRange(SlotCount, slots.Count - SlotCount);
                SetDirty();
            }
            else if(SlotCount > slots.Count)
            {
                LogManager.LogWarning($"存档槽配置数量从 {slots.Count} 增加到 {SlotCount}");
                var startCount = slots.Count;
                for (var i = startCount; i < SlotCount; i++)
                {
                    slots.Add(new ArchiveSlotInfo(CreateSlotKey(i)));
                }
                SetDirty();
            }
        }


        /// <summary>
        /// 根据索引获取槽位信息
        /// </summary>
        public ArchiveSlotInfo GetSlotInfo(int slotIndex)
        {
            if (slots == null || slotIndex >= slots.Count)
            {
                LogManager.LogWarning($"无法获取槽位索引{slotIndex}的信息");
                return null;
            }
            return slots[slotIndex];
        }

        /// <summary>
        /// 记录槽位保存时间
        /// </summary>
        public void MarkSlotSaveTime(int slotIndex)
        {
            var slotInfo = GetSlotInfo(slotIndex);
            slotInfo.SaveTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            slotInfo.hasArchive = true;
            SetDirty();
        }


        /// <summary>
        /// 设置指定槽位的显示名称
        /// </summary>
        public void SetSlotDisplayName(int slotIndex, string displayName)
        {
            var slotInfo = GetSlotInfo(slotIndex);
            slotInfo.displayName = displayName;
            SetDirty();
        }

        /// <summary>
        /// 清理指定槽位的显示信息
        /// </summary>
        public void ClearSlot(int slotIndex)
        {
            var slotInfo = GetSlotInfo(slotIndex);
            slotInfo.displayName = null;
            slotInfo.hasArchive = false;
            slotInfo.SaveTime = 0;
            SetDirty();
        }

        /// <summary>
        /// 设置当前槽位索引，并更新该槽位的显示名称
        /// </summary>
        public void SetCurrentSlot(int slotIndex, string displayName = null)
        {
            var slotInfo = GetSlotInfo(slotIndex);
            slotInfo.displayName = displayName;
            currentSlotIndex = slotIndex;
            SetDirty();
        }
        
        /// <summary>
        /// 加载存档
        /// </summary>
        internal void LoadSlot(int slotIndex)
        {
            if (currentSlotIndex == slotIndex)
            {
                return;
            }
            currentSlotIndex = slotIndex;
            SetDirty();
        }

        /// <summary>
        /// 根据索引生成槽位唯一标识
        /// </summary>
        private string CreateSlotKey(int slotIndex)
        {
            return $"Slot_{slotIndex + 1:00}";
        }
    }
}
