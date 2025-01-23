using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum MaterialTargetComponent{
    SpriteRenderer,
    Image,
}

[System.Serializable]
public class MaterialReplaceRule{
    [LabelText("材质的目标组件")]
    public MaterialTargetComponent targetComponent;
    public string path;
    [LabelText("需要替换的材质")]
    public Material needReplacedMaterial;
    [LabelText("目标材质")]
    public Material targetMaterial;
}

[CreateAssetMenu(fileName = "ChangeMaterialTool", menuName = "创建.Assets文件/ChangeMaterialTool")]
public class ChangeMaterialTool : SerializedScriptableObject
{
    [FoldoutGroup("按照GameObject查找")]
    public GameObject[] prefabs;
    [LabelText("材质的目标组件")]
    [FoldoutGroup("按照GameObject查找")]
    public MaterialTargetComponent targetComponent;
    [LabelText("需要替换的材质")]
    [FoldoutGroup("按照GameObject查找")]
    public Material needReplacedMaterial;
    [LabelText("目标材质")]
    [FoldoutGroup("按照GameObject查找")]
    public Material targetMaterial;

    [FoldoutGroup("按照路径查找")]
    public MaterialReplaceRule[] prefabsPath;


    private void Replace(GameObject prefab){
        Replace(targetComponent, prefab, needReplacedMaterial, targetMaterial);
        var render = prefab.GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in render)
        {
            if(item.sharedMaterial == needReplacedMaterial){
                item.sharedMaterial = targetMaterial;
            }
        }
        PrefabUtility.RecordPrefabInstancePropertyModifications(prefab);
    }

    private void Replace(MaterialReplaceRule materialReplaceRule){
        string[] prefabPaths = Directory.GetFiles(materialReplaceRule.path, "*.prefab", SearchOption.AllDirectories);
        var count = prefabPaths.Length;
        var index = 0f;
        foreach (string item in prefabPaths)
        {
            Debug.Log("预制体"+item);
            index++;
            EditorUtility.DisplayProgressBar("替换材质中", item, index/count);
            try
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(item);
                Replace(materialReplaceRule.targetComponent, prefab, materialReplaceRule.needReplacedMaterial, materialReplaceRule.targetMaterial);
                PrefabUtility.RecordPrefabInstancePropertyModifications(prefab);
            }
            catch{
                Debug.LogError($"{item}加载失败，可能预制已经损坏");
            }
        }
    }

    private void Replace(MaterialTargetComponent targetComponent, GameObject prefab, Material needReplacedMaterial, Material targetMaterial){
        switch (targetComponent)
        {
            case MaterialTargetComponent.SpriteRenderer:
                Replace(prefab.GetComponentsInChildren<SpriteRenderer>(), needReplacedMaterial, targetMaterial);
                break;
            case MaterialTargetComponent.Image:
                Replace(prefab.GetComponentsInChildren<Image>(), needReplacedMaterial, targetMaterial);
                break;
        }
    }

    private void Replace(SpriteRenderer[] spriteRenderers, Material needReplacedMaterial, Material targetMaterial){
        foreach (var renderItem in spriteRenderers)
        {
            if(needReplacedMaterial == null){
                if(renderItem.sharedMaterial == null || renderItem.sharedMaterial.name == "Sprites-Default"){
                    renderItem.sharedMaterial = targetMaterial;
                }
            }
            else{
                if(renderItem.sharedMaterial == needReplacedMaterial){
                    renderItem.sharedMaterial = targetMaterial;
                }
            }
        }
    }
    private void Replace(Image[] images, Material needReplacedMaterial, Material targetMaterial){
        foreach (var renderItem in images)
        {
            if(needReplacedMaterial == null){
                if(renderItem.material == null || renderItem.material.name == "Default UI Material"){
                    renderItem.material = targetMaterial;
                }
            }
            else{
                if(renderItem.material == needReplacedMaterial){
                    renderItem.material = targetMaterial;
                }
            }
        }
    }

    [FoldoutGroup("按照GameObject查找")]
    [OnInspectorGUI]
    private void ChangeMaterialBtn_Obj()
    {
        using (new GUILayout.HorizontalScope())
        {
            GUI.color = new Color(0.5f, 0.8f, 1);
            if (GUILayout.Button("更换材质", GUILayout.Height(30)))
            {
                ReplaceMaterial_Obj();
            }

            GUI.color = new Color(1, 0.5f, 0.5f);
            if (GUILayout.Button("清空列表数据", GUILayout.Height(30)))
            {
                ClearData_Obj();
            }
            GUI.color = Color.white;
        }
    }

    private void ReplaceMaterial_Obj(){

        var count = prefabs.Length;
        var index = 0;
        foreach (var item in prefabs)
        {
            Replace(item);
            index++;
            EditorUtility.DisplayProgressBar("替换材质中", item.name, index/count);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
    }

    private void ClearData_Obj(){
        prefabs = new GameObject[]{};
    }

    [FoldoutGroup("按照路径查找")]
    [OnInspectorGUI]
    private void ChangeMaterialBtn_Path(){
        using (new GUILayout.HorizontalScope())
        {
            GUI.color = new Color(0.5f, 0.8f, 1);
            if (GUILayout.Button("更换材质", GUILayout.Height(30)))
            {
                ReplaceMaterial_Path();
            }

            GUI.color = new Color(1, 0.5f, 0.5f);
            if (GUILayout.Button("清空列表数据", GUILayout.Height(30)))
            {
                ClearData_Path();
            }
        }
    }

    private void ReplaceMaterial_Path(){
        foreach (var item in prefabsPath)
        {
            Replace(item);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
    }

    private void ClearData_Path(){
        prefabsPath = new MaterialReplaceRule[]{};
    }
}
