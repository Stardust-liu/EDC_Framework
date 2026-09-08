using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ArchiveData{

    public class GameArchive
    {
        private static GameArchive instance;
        private static GameArchive Instance{
            get{
                instance ??= new();
                return instance;
            }
        }
        private readonly Dictionary<Type, ArchiveKeyAttribute> archiveAttributeCache;
        private readonly Dictionary<Type, BaseGameArchive> globalArchiveData;
        private readonly Dictionary<Type, BaseGameArchive> slotArchiveData;
        private readonly Dictionary<Type, string> globalArchivePath;
        private readonly Dictionary<Type, string> slotArchivePath;
        private Action<string> onSlotDataSaved;
        private const string dataParentFolder = "Data";
        private const string slotsFolder = "Slots";
        private string currentSlotKey;

        private GameArchive()
        {
            archiveAttributeCache = new();
            globalArchiveData = new();
            slotArchiveData = new();
            globalArchivePath = new();
            slotArchivePath = new();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public static T GetData<T>()where T : BaseGameArchive, new()
        {
            return Instance.Get<T>();
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public static void ReleaseData<T>() where T : BaseGameArchive, new()
        {
            Instance.Release<T>();
        }

        /// <summary>
        /// 立刻保存数据
        /// </summary>
        public static void SaveDataNow<T>()where T : BaseGameArchive, new()
        {
            Instance.Save<T>();
        }

        /// <summary>
        /// 异步保存数据
        /// </summary>
        public static async Task SaveDataAsync<T>()where T : BaseGameArchive, new()
        {
            await Task.Run(Instance.Save<T>);
        }

        /// <summary>
        /// 保存所有已修改的数据
        /// </summary>
        public static void SaveAllDirtyData()
        {
            Instance.SaveAll();
        }

        /// <summary>
        /// 将当前游戏进度保存到指定槽位，并在后续读写使用该槽位
        /// </summary>
        public static void SaveToSlot(string slotKey)
        {
            Instance.SaveToSlotInternal(slotKey);
        }

        /// <summary>
        /// 加载存档
        /// </summary>
        public static void LoadSlot(string slotKey)
        {
            Instance.LoadSlotInternal(slotKey);
        }

        /// <summary>
        /// 删除指定存档槽
        /// </summary>
        internal static bool DeleteSlot(string slotKey)
        {
            if (string.IsNullOrEmpty(slotKey))
            {
                return false;
            }

            return Instance.DeleteSlotData(slotKey);
        }

        /// <summary>
        /// 设置存档槽数据保存完成后的回调。
        /// </summary>
        internal static void SetSlotDataSavedCallback(Action<string> onSaved)
        {
            Instance.onSlotDataSaved = onSaved;
        }

        private void LoadSlotInternal(string slotKey)
        {
            if (currentSlotKey == slotKey)
            {
                return;
            }
            slotArchiveData.Clear();
            slotArchivePath.Clear();
            currentSlotKey = slotKey;
        }

        private void SaveToSlotInternal(string _currentSlotKey)
        {
            if (currentSlotKey != _currentSlotKey)
            {
                var sourceFolder = GetSlotFolderPath(currentSlotKey);
                var targetFolder = GetSlotFolderPath(_currentSlotKey);
                Directory.CreateDirectory(targetFolder);

                // 复制原槽文件，保留尚未加载或未修改的数据。
                if (Directory.Exists(sourceFolder))
                {
                    foreach (var filePath in Directory.GetFiles(sourceFolder))
                    {
                        File.Copy(filePath, Path.Combine(targetFolder, Path.GetFileName(filePath)), true);
                    }
                }

                // 清除目标槽独有的旧文件，避免混入另一份进度。
                foreach (var filePath in Directory.GetFiles(targetFolder))
                {
                    if (!File.Exists(Path.Combine(sourceFolder, Path.GetFileName(filePath))))
                    {
                        File.Delete(filePath);
                    }
                }
                currentSlotKey = _currentSlotKey;
                slotArchivePath.Clear();
            }
            SaveAll(slotArchiveData);
            onSlotDataSaved?.Invoke(currentSlotKey);
        }

        private void Save<T>() where T : BaseGameArchive, new()
        {
            var type = typeof(T);
            var data = Get<T>();
            if (data == null || !data.IsDirty)
            {
                return;
            }
            Save(type, data);
            if (GetArchiveDomain(type).Domain == ArchiveDomain.Slot)
            {
                onSlotDataSaved?.Invoke(currentSlotKey);
            }
        }

        private void Save(Type type, BaseGameArchive data)
        {
            data.OnBeforeSave();
            File.WriteAllText(GetDataPath(type), JsonConvert.SerializeObject(data));
            data.ClearDirty();
        }

        private void SaveAll()
        {
            SaveAll(globalArchiveData);
            SaveAll(slotArchiveData);
            onSlotDataSaved?.Invoke(currentSlotKey);
        }

        /// <summary>
        /// 只保存已修改的数据，返回是否实际写入过数据文件。
        /// </summary>
        private void SaveAll(Dictionary<Type, BaseGameArchive> archiveData)
        {
            var types = new List<Type>(archiveData.Keys);
            foreach (var type in types)
            {
                if (!archiveData.TryGetValue(type, out var data))
                {
                    continue;
                }

                if (!data.IsDirty)
                {
                    continue;
                }

                Save(type, data);
            }
        }

        private T Get<T>()where T : BaseGameArchive, new()
        {
            var type = typeof(T);
            var archiveData = GetArchiveDataDictionary(type);
            if(!archiveData.ContainsKey(type)){
                var dataPath = GetDataPath(type);
                archiveData[type] = GetData<T>(dataPath);
            }
            return (T)archiveData[type];
        }

        private T GetData<T>(string dataPath)where T : BaseGameArchive, new()
        {
            T data;
            if(File.Exists(dataPath)){
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(dataPath));
                if (data == null)
                {
                    data = new T();
                    data.OnCreateDefaultData();
                }
                else
                {
                    data.OnAfterLoad();
                }
            }
            else{
                data = new T();
                data.OnCreateDefaultData();
            }
            return data;
        }

        private void Release<T>()where T : BaseGameArchive, new()
        {
            var type = typeof(T);
            GetArchiveDataDictionary(type).Remove(type);
            GetArchivePathDictionary(type).Remove(type);
        }

        private bool DeleteSlotData(string deleteSlotKey)
        {
            if (currentSlotKey == deleteSlotKey)
            {
                LogManager.LogWarning("当前存档槽正在使用中，无法被删除");
                return false;
            }

            var slotPath = GetSlotFolderPath(deleteSlotKey);
            if (Directory.Exists(slotPath))
            {
                Directory.Delete(slotPath, true);
            }
            return true;
        }

        /// <summary>
        /// 获取存档文件完整路径。未缓存时创建所在目录，并缓存到对应的数据域字典。
        /// 无法确定路径时返回null，由调用方停止本次读写。
        /// </summary>
        private string GetDataPath(Type type)
        {
            var archiveDomain = GetArchiveDomain(type);
            var archivePath = GetArchivePathDictionary(type);
            if (archivePath.TryGetValue(type, out var dataPath))
            {
                return dataPath;
            }

            var folderPath = archiveDomain.Domain == ArchiveDomain.Slot ? GetSlotFolderPath(currentSlotKey) : GetGlobalFolderPath();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            dataPath = Path.Combine(folderPath, archiveDomain.Key);
            archivePath[type] = dataPath;
            return dataPath;
        }


        private Dictionary<Type, BaseGameArchive> GetArchiveDataDictionary(Type type)
        {
            return GetArchiveDomain(type).Domain == ArchiveDomain.Slot ? slotArchiveData : globalArchiveData;
        }

        private Dictionary<Type, string> GetArchivePathDictionary(Type type)
        {
            return GetArchiveDomain(type).Domain == ArchiveDomain.Slot ? slotArchivePath : globalArchivePath;
        }


        private ArchiveKeyAttribute GetArchiveDomain(Type type)
        {
            if (archiveAttributeCache.TryGetValue(type, out var domain))
            {
                return domain;
            }
            var archiveKey = Attribute.GetCustomAttribute(type, typeof(ArchiveKeyAttribute)) as ArchiveKeyAttribute;
            if (archiveKey == null)
            {
                LogManager.LogWarning($"{type.Name} 缺少 ArchiveKeyAttribute");
            }
            domain = archiveKey;
            archiveAttributeCache.Add(type, domain);
            return domain;
        }

        private string GetSlotFolderPath(string SlotKey)
        {
            return Path.Combine(Application.persistentDataPath, dataParentFolder, slotsFolder, SlotKey);
        }

        private string GetGlobalFolderPath()
        {
            return Path.Combine(Application.persistentDataPath, dataParentFolder);
        }
    }
}
