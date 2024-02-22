using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;

public abstract class BaseGameArchive<T>
where T : new()
{
    protected T data;
    protected string dataFileName;
    private string filePath;
    private bool isSaveFinish;
    private bool isCleanFinish;
    private bool isNeedSaveAgain;
    private const string dataParentFolder = "Data";

    public BaseGameArchive(){
        isSaveFinish = true;
        isCleanFinish = true;
        InitFilePath();
    }

    protected virtual void InitFilePath(){
        var folderPath = Path.Combine(Application.persistentDataPath, dataParentFolder);
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }
        filePath = Path.Combine(folderPath, dataFileName);
    }
    
    /// <summary>
    /// 读取数据
    /// </summary>
    public void ReadData()
    {
        if(File.Exists(filePath)){
            data =  JsonMapper.ToObject<T>(File.ReadAllText(filePath));
        }
        else{
            data = new T();
        }
    }

    /// <summary>
    /// 立即保存
    /// </summary>
    public void SaveDataNow()
    {
        string json = JsonMapper.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public void SaveDataAsync(){
        if(isSaveFinish){
            isSaveFinish = false;
            var task = InternalSaveDataAsync();
            task.GetAwaiter().OnCompleted(SaveDataAsyncFinish);
        }
        else{
            isNeedSaveAgain = true;
        }
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void CleanData(){
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
            string json = JsonMapper.ToJson(data);
            File.WriteAllText(filePath, json);
        }

        await Task.Run(saveData);
    }

    private void SaveDataAsyncFinish(){
        isSaveFinish = true;
        if(isNeedSaveAgain){
            isNeedSaveAgain = false;
            SaveDataAsync();
        }
    }

    private void CleanFinish(){
        isCleanFinish = true;
    }
}
