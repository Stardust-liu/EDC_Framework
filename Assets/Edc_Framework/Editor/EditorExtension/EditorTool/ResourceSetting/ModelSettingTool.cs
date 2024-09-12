using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ModelSettingTool", menuName = "创建.Assets文件/素材设置工具/ModelSettingTool")]
public class ModelSettingTool : SerializedScriptableObject
{
    [FolderPath, LabelText("模型路径")]
    public List<string> modelPath;
    [FolderPath, LabelText("带动画的模型路径"),LabelWidth(105)]
    public string animationPath;
    [LabelText("勾选了Read/Write的资源"),HideLabel, ReadOnly, GUIColor(1, 0.7f, 0.7f)]
    public List<GameObject> readWriteList;


    [Button("设置模型为指定格式", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void ApplicationSetting(){
        readWriteList ??= new List<GameObject>();
        readWriteList.Clear();
        FindModel(modelPath);
    }

    private void FindModel(List<string> pathList){
         if(pathList == null)
            return;
        var count = pathList.Count;
        for (var i = 0; i < count; i++)
        {
            FindModel(pathList[i]);
        }

        void FindModel(string path){
            if(AssetDatabase.IsValidFolder(path)){
                var modelPaths = Directory.GetFiles(path, "*.fbx", SearchOption.AllDirectories);
                foreach (string item in modelPaths)
                {
                    SetModelSetting(item, AssetDatabase.LoadAssetAtPath<GameObject>(item));
                }
            }
            else{
                Debug.LogError($"不存在 {path} 路径");
            }
        }
    }

    private void SetModelSetting(string path, GameObject model){
        ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(path);
        modelImporter.meshOptimizationFlags = MeshOptimizationFlags.Everything;
        if(modelImporter.isReadable){
            readWriteList.Add(model);
        }
        modelImporter.importNormals = ModelImporterNormals.Import;
        modelImporter.importTangents = ModelImporterTangents.None;
        modelImporter.importCameras = false;
        modelImporter.importLights = false;

        //modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
        SetAnimation(path, modelImporter);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    /// <summary>
    /// 设置动画模块
    /// </summary>
    private void SetAnimation(string path, ModelImporter modelImporter){
        if(path.StartsWith(animationPath)){
            modelImporter.importAnimation = true;
            modelImporter.optimizeGameObjects = true;
            modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
            modelImporter.animationRotationError = 0.1f;
            modelImporter.animationPositionError = 0.5f;
            modelImporter.animationScaleError = 1.0f;
        }
        else{
            modelImporter.importAnimation = false;
            modelImporter.animationType = ModelImporterAnimationType.None;
        }
    }
}
