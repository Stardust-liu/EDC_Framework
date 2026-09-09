using System;
using System.Collections.Generic;
using System.IO;
using ArchiveData;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ArchiveTool", menuName = "创建.Assets文件/FrameworkTool/ArchiveTool")]
public class ArchiveTool : SerializedScriptableObject
{
    private const string DataFolderName = "Data";
    private const string SlotsFolderName = "Slots";
    private const string ArchiveInfoFileName = "ArchiveInfo";

    [ShowInInspector, ReadOnly, PropertyOrder(-1), LabelText("存档目录")]
    private string ArchiveFolderPath => Path.Combine(Application.persistentDataPath, DataFolderName);

    private string SlotsFolderPath => Path.Combine(ArchiveFolderPath, SlotsFolderName);

    [NonSerialized]
    private readonly List<string> globalArchiveFiles = new();

    [NonSerialized]
    private readonly List<string> slotArchiveFiles = new();

    [NonSerialized]
    private readonly List<string> slotKeys = new();

    [NonSerialized]
    private string selectedSlotKey;

    [ShowInInspector, PropertyOrder(10), FoldoutGroup("全局存档", Expanded = true), LabelText("存档文件")]
    [ListDrawerSettings(ShowFoldout = false, IsReadOnly = true, ShowPaging = false)]
    private List<string> GlobalArchiveFiles => globalArchiveFiles;

    [ShowInInspector, PropertyOrder(20), FoldoutGroup("槽位存档", Expanded = true)]
    [LabelText("查看槽位")]
    [ValueDropdown(nameof(GetSlotKeys))]
    private string SelectedSlotKey
    {
        get => selectedSlotKey;
        set
        {
            if (selectedSlotKey == value)
            {
                return;
            }

            selectedSlotKey = value;
            RefreshSlotArchiveFiles();
        }
    }

    [ShowInInspector, PropertyOrder(21), FoldoutGroup("槽位存档", Expanded = true), LabelText("存档文件")]
    [ListDrawerSettings(ShowFoldout = false, IsReadOnly = true, ShowPaging = false)]
    private List<string> SlotArchiveFiles => slotArchiveFiles;

    private void OnEnable()
    {
        RefreshArchiveFiles();
    }

    [PropertyOrder(0), ButtonGroup("存档操作")]
    [Button("打开存档文件夹", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void OpenArchiveFolder()
    {
        Directory.CreateDirectory(ArchiveFolderPath);
        RefreshArchiveFiles();
        EditorUtility.RevealInFinder(ArchiveFolderPath);
    }

    [PropertyOrder(1)]
    [Button("重新扫描存档文件", ButtonSizes.Medium)]
    private void RefreshArchiveFiles()
    {
        RefreshGlobalArchiveFiles();
        RefreshSlotKeys();
        RefreshSlotArchiveFiles();
    }

    [PropertyOrder(0), ButtonGroup("存档操作")]
    [Button("清空存档", ButtonSizes.Large), GUIColor(1f, 0.5f, 0.5f)]
    private void ClearArchiveFiles()
    {
        if (!Directory.Exists(ArchiveFolderPath))
        {
            ClearPreview();
            return;
        }

        if (!EditorUtility.DisplayDialog("注意！", "确认清空项目存档吗？", "确认", "取消"))
        {
            return;
        }

        Directory.Delete(ArchiveFolderPath, true);
        ClearPreview();
    }

    private void RefreshGlobalArchiveFiles()
    {
        globalArchiveFiles.Clear();
        if (!Directory.Exists(ArchiveFolderPath))
        {
            return;
        }

        foreach (var filePath in Directory.GetFiles(ArchiveFolderPath, "*", SearchOption.TopDirectoryOnly))
        {
            globalArchiveFiles.Add(Path.GetFileName(filePath));
        }
        globalArchiveFiles.Sort(StringComparer.OrdinalIgnoreCase);
    }

    private void RefreshSlotKeys()
    {
        slotKeys.Clear();
        AddSlotKeysFromArchiveInfo();

        if (Directory.Exists(SlotsFolderPath))
        {
            foreach (var slotFolderPath in Directory.GetDirectories(SlotsFolderPath))
            {
                AddSlotKey(Path.GetFileName(slotFolderPath));
            }
        }

        slotKeys.Sort(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(selectedSlotKey) || !ContainsSlotKey(selectedSlotKey))
        {
            selectedSlotKey = slotKeys.Count > 0 ? slotKeys[0] : null;
        }
    }

    private void AddSlotKeysFromArchiveInfo()
    {
        var archiveInfoPath = Path.Combine(ArchiveFolderPath, ArchiveInfoFileName);
        if (!File.Exists(archiveInfoPath))
        {
            return;
        }

        try
        {
            var archiveInfo = JsonConvert.DeserializeObject<ArchiveInfoData>(File.ReadAllText(archiveInfoPath));
            if (archiveInfo?.slots == null)
            {
                return;
            }

            foreach (var slotInfo in archiveInfo.slots)
            {
                AddSlotKey(slotInfo?.slotKey);
            }
        }
        catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException || exception is JsonException)
        {
            Debug.LogWarning($"读取存档槽信息失败：{exception.Message}");
        }
    }

    private void AddSlotKey(string slotKey)
    {
        if (string.IsNullOrEmpty(slotKey) || ContainsSlotKey(slotKey))
        {
            return;
        }

        slotKeys.Add(slotKey);
    }

    private bool ContainsSlotKey(string slotKey)
    {
        return slotKeys.Exists(item => string.Equals(item, slotKey, StringComparison.OrdinalIgnoreCase));
    }

    private IEnumerable<string> GetSlotKeys()
    {
        return slotKeys;
    }

    private void RefreshSlotArchiveFiles()
    {
        slotArchiveFiles.Clear();
        if (string.IsNullOrEmpty(selectedSlotKey))
        {
            return;
        }

        var slotFolderPath = Path.Combine(SlotsFolderPath, selectedSlotKey);
        if (!Directory.Exists(slotFolderPath))
        {
            return;
        }

        foreach (var filePath in Directory.GetFiles(slotFolderPath, "*", SearchOption.AllDirectories))
        {
            slotArchiveFiles.Add(Path.GetRelativePath(slotFolderPath, filePath));
        }

        slotArchiveFiles.Sort(StringComparer.OrdinalIgnoreCase);
    }

    private void ClearPreview()
    {
        globalArchiveFiles.Clear();
        slotArchiveFiles.Clear();
        slotKeys.Clear();
        selectedSlotKey = null;
    }
}
