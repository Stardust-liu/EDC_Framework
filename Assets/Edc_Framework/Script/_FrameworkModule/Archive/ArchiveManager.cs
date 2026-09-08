using System.Collections.Generic;
using ArchiveData;
using UnityEngine;

public class ArchiveManager : BaseIOCComponent<ArchiveInfoData>
{
    /// <summary>
    /// 当前允许使用的存档槽数量
    /// </summary>
    public int SlotCount => Data.SlotCount;

    /// <summary>
    /// 当前使用的存档槽索引
    /// </summary>
    public int CurrentSlotIndex => Data.currentSlotIndex;

    /// <summary>
    /// 当前使用的存档槽
    /// 主菜单继续游戏、读取存档、游戏内保存都会围绕这个槽位工作
    /// </summary>
    public string CurrentSlotKey => Data.CurrentSlotKey;

    protected override void Init()
    {
        base.Init();
        GameArchive.SetSlotDataSavedCallback(UpdateCurrentSloatSaveTime);
        GameArchive.LoadSlot(Data.CurrentSlotKey);
        GameArchive.SaveDataNow<ArchiveInfoData>();
    }

    /// <summary>
    /// 获取全部存档槽信息
    /// </summary>
    public IReadOnlyList<ArchiveSlotInfo> GetSlotInfos()
    {
        var result = new List<ArchiveSlotInfo>(SlotCount);
        for (var i = 0; i < SlotCount; i++)
        {
            result.Add(Data.GetSlotInfo(i));
        }
        return result;
    }

    /// <summary>
    /// 获取指定存档槽信息
    /// </summary>
    public ArchiveSlotInfo GetSlotInfo(int slotIndex)
    {
        if (!CheckSlotIndex(slotIndex))
        {
            return null;
        }
        var slotInfo = Data.GetSlotInfo(slotIndex);
        if (Data.GetSlotInfo(slotIndex) == null)
        {
            LogManager.Log($"存档槽索引{slotIndex}中无有效信息");
            return null;
        }
        else
        {
            return slotInfo;
        }
    }

    /// <summary>
    /// 获取指定槽位显示名称
    /// </summary>
    public string GetSlotName(int slotIndex)
    {
        return GetSlotInfo(slotIndex).displayName;
    }

    /// <summary>
    /// 判断指定索引的存档槽是有存档数据
    /// </summary>
    public bool IsSlotEmpty(int slotIndex)
    {
        return GetSlotInfo(slotIndex).hasArchive;
    }

    /// <summary>
    /// 判断当前槽是否有存档
    /// 主菜单的继续游戏按钮可以用这个结果决定是否显示或禁用
    /// </summary>
    public bool HasCurrentSlotArchive()
    {
        return GetSlotInfo(CurrentSlotIndex).hasArchive;
    }

    /// <summary>
    /// 将当前游戏进度保存到指定槽位，并在后续读写使用该槽位
    /// </summary>
    public void SaveToSlot(int slotIndex, string displayName = null)
    {
        if (!CheckSlotIndex(slotIndex))
        {
            return;
        }
        Data.SetCurrentSlot(slotIndex, displayName);
        GameArchive.SaveToSlot(Data.GetSlotInfo(slotIndex).slotKey);
        GameArchive.SaveDataNow<ArchiveInfoData>();
    }

    /// <summary>
    /// 加载存档
    /// </summary>
    public bool LoadSlot(int slotIndex)
    {
        if (!CheckSlotIndex(slotIndex))
        {
            return false;
        }
        var slotInfo = Data.GetSlotInfo(slotIndex);
        if (!slotInfo.hasArchive)
        {
            LogManager.LogWarning($"存档槽 {slotIndex} 中没有存档，无法加载");
            return false;
        }
        GameArchive.LoadSlot(slotInfo.slotKey);
        Data.LoadSlot(slotIndex);
        GameArchive.SaveDataNow<ArchiveInfoData>();
        return true;
    }

    /// <summary>
    /// 设置指定槽位的显示名称
    /// </summary>
    public void SetSlotName(int slotIndex, string displayName)
    {
        if (!CheckSlotIndex(slotIndex))
        {
            return;
        }
        Data.SetSlotDisplayName(slotIndex, displayName);
        GameArchive.SaveDataNow<ArchiveInfoData>();
    }


    /// <summary>
    /// 删除指定存档槽的数据
    /// </summary>
    public bool DeleteSlot(int slotIndex)
    {
        if (!CheckSlotIndex(slotIndex))
        {
            return false;
        }

        if (GameArchive.DeleteSlot(GetSlotInfo(slotIndex).slotKey))
        {
            Data.ClearSlot(slotIndex);
            GameArchive.SaveDataNow<ArchiveInfoData>();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 刷新指定槽位保存时间
    /// </summary>
    private void UpdateCurrentSloatSaveTime(string slotKey)
    {
        Data.MarkSlotSaveTime(CurrentSlotIndex);
        GameArchive.SaveDataNow<ArchiveInfoData>();
    }

    private bool CheckSlotIndex(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < SlotCount)
        {
            return true;
        }

        LogManager.LogWarning($"存档槽索引 {slotIndex} 不在有效范围内");
        return false;
    }
}
