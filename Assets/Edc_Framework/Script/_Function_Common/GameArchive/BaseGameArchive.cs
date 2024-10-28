using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
namespace ArchiveData{
    public class BaseGameArchive<T> : IGameArchive
    where T : new()
    {
        protected static T data;
        private static string filePath;
        private static bool isSaveFinish;
        private static bool isCleanFinish;
        private const string dataParentFolder = "Data";
        public bool isInitSave = false;

        /// <summary>
        /// 读取数据
        /// </summary>
        public static T ReadData()
        {
            InitFilePath();
            if(File.Exists(filePath)){
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            else{
                data = new T();
            }
            return data;
        }

        /// <summary>
        /// 立即保存
        /// </summary>
        public void SaveDataNow()
        {
            if(FrameworkManager.IsSaveDisabled){
                return;
            }
            isInitSave = true;
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }
        
        /// <summary>
        /// 异步保存
        /// </summary>
        public void SaveDataAsync(){
            if(FrameworkManager.IsSaveDisabled){
                return;
            }
            if(isSaveFinish){
                isSaveFinish = false;
                isInitSave = true;
                var task = InternalSaveDataAsync();
                task.GetAwaiter().OnCompleted(SaveDataAsyncFinish);
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void IGameArchive.CleanData(){
            if(isCleanFinish){
                isCleanFinish = false;
                data = new T();
                var task = InternalSaveDataAsync();
                task.GetAwaiter().OnCompleted(CleanFinish);
            }
        }

        /// <summary>
        /// 异步保存
        /// </summary>
        private async Task InternalSaveDataAsync(){
            void saveData(){
                string json = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, json);
            }
            await Task.Run(saveData);
        }

        /// <summary>
        /// 初始化保存文件路径
        /// </summary>
        private static void InitFilePath(){
            var dataFileName = typeof(T).Name;
            var folderPath = Path.Combine(Application.persistentDataPath, Application.productName, dataParentFolder);
            if(!Directory.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }
            filePath = Path.Combine(folderPath, dataFileName);
        }


        private void SaveDataAsyncFinish(){
            isSaveFinish = true;
        }

        private void CleanFinish(){
            isCleanFinish = true;
        }
    }
}

