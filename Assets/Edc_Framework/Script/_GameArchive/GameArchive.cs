using System;
using System.Collections;
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
                if(instance == null){
                    instance = new();
                    instance.gameArchiveData = new();
                    instance.gameArchivePath = new();
                }
                return instance;
            }
        }
        private Dictionary<Type, BaseGameArchive> gameArchiveData;
        private Dictionary<Type, string> gameArchivePath;
        private const string dataParentFolder = "Data";


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
            await Task.Run(instance.Save<T>);
        }

        private void Save<T>() where T : BaseGameArchive, new()
        {
            var type = typeof(T);
            var json = JsonConvert.SerializeObject(gameArchiveData[type]);
            File.WriteAllText(gameArchivePath[type], json);
        }

        private T Get<T>()where T : BaseGameArchive, new()
        {
            var type = typeof(T);
            if(!gameArchivePath.ContainsKey(type)){
                var dataPath = GetDataPath<T>();
                gameArchivePath.Add(type, dataPath);
                gameArchiveData.Add(type, GetData<T>(dataPath));
            }   
            return (T)gameArchiveData[type];
        }

        private void Release<T>()where T : BaseGameArchive, new()
        {
            var type = typeof(T);
            gameArchiveData.Remove(type);
            gameArchivePath.Remove(type);
        }

        private string GetDataPath<T>()where T : BaseGameArchive
        {
            var dataFileName = typeof(T).Name;
            var folderPath = Path.Combine(Application.persistentDataPath, dataParentFolder);
            if(!Directory.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }
            return Path.Combine(folderPath, dataFileName);
        }

        private T GetData<T>(string dataPath)where T : BaseGameArchive, new()
        {
            T data;
            if(File.Exists(dataPath)){
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(dataPath));
            }
            else{
                data = new T();
                ((IBaseGameArchive)data).InitData();
            }
            return data;
        }
    }
}
