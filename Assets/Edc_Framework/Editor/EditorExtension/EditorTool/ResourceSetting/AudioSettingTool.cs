using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioSettingTool", menuName = "创建.Assets文件/素材设置工具/AudioSettingTool")]
public class AudioSettingTool : SerializedScriptableObject
{
    [FolderPath, FoldoutGroup("高质量音频路径"), LabelText("立刻播放")]
    public List<string> highQualityPath_LL;
    [FolderPath, FoldoutGroup("高质量音频路径"), LabelText("延迟播放")]
    public List<string> highQualityPath_HL;

    [FoldoutGroup("低质量音频路径"),FolderPath, LabelText("立刻播放")]
    public List<string> lowQualityPath_LL;
    [FoldoutGroup("低质量音频路径"),FolderPath, LabelText("延迟播放")]
    public List<string> lowQualityPath_HL;
    [LabelText("双声道音频")]
    public AudioClip[] stereoList;
    private static HashSet<string> stereoListPath;

    private void SetStereoListPath(){
        if(stereoList != null){
            var count = stereoList.Length;
            stereoListPath = new HashSet<string>();
            for (var i = 0; i < count; i++)
            {
                var path = AssetDatabase.GetAssetPath(stereoList[i]);
                if(!stereoListPath.Contains(path)){
                    stereoListPath.Add(path);
                }
                else{
                    Debug.LogWarning($"双声道音频资源重复，素材为 {stereoList[i]}");
                }
            }
        }
    }

    
    [Button("设置音频为指定格式", ButtonSizes.Large), GUIColor(0.5f, 0.8f, 1f)]
    private void ApplicationSetting(){
        SetStereoListPath();
        FindAudio(highQualityPath_LL, true, false);
        FindAudio(highQualityPath_HL, true, true);
        FindAudio(lowQualityPath_LL, false, false);
        FindAudio(lowQualityPath_HL, false, true);
    }

    private void FindAudio(List<string> pathList, bool ishighQuality, bool isAllowPlaybackLatency){
        if(pathList == null)
            return;
        var count = pathList.Count;
        for (var i = 0; i < count; i++)
        {
            FindAudio(pathList[i], ishighQuality, isAllowPlaybackLatency);
        }

        static void FindAudio(string path, bool ishighQuality, bool isAllowPlaybackLatency){
            if(AssetDatabase.IsValidFolder(path)){
                var mp3Paths = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                var wavPaths = Directory.GetFiles(path, "*.wav", SearchOption.AllDirectories);
                var allAudioPaths = mp3Paths.Concat(wavPaths).ToArray();
                foreach (string item in allAudioPaths)
                {
                    SetTextureSetting(ishighQuality, isAllowPlaybackLatency, item, AssetDatabase.LoadAssetAtPath<AudioClip>(item));
                }
            }
            else{
                Debug.LogError($"不存在 {path} 路径");
            }
        }
    }

    private static void SetTextureSetting(bool isHighQuality, bool isAllowPlaybackLatency, string path, AudioClip clip){
        AudioImporter audioImporter = (AudioImporter)AssetImporter.GetAtPath(path);
        audioImporter.forceToMono  = !stereoListPath.Contains(path);
        AudioImporterSampleSettings Setting = SetLoadTypeAndFormatType(clip.length, isAllowPlaybackLatency);
        Setting.sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate;
        Setting.sampleRateOverride = (uint)(isHighQuality ? 44100: 22050);
        audioImporter.defaultSampleSettings = Setting;
#if UNITY_STANDALONE
        audioImporter.SetOverrideSampleSettings("Standalone", Setting);
#elif UNITY_ANDROID
        audioImporter.SetOverrideSampleSettings("Android", Setting);
#elif UNITY_IOS
        audioImporter.SetOverrideSampleSettings("iOS", Setting);
#elif UNITY_SWITCH
        audioImporter.SetOverrideSampleSettings("Switch", Setting);
#endif
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    /// <summary>
    /// 设置压缩格式和加载方式
    /// </summary>
    private static AudioImporterSampleSettings SetLoadTypeAndFormatType(float clipLength, bool isAllowPlaybackLatency)
    {
        var setting = new AudioImporterSampleSettings();
        if(clipLength >= 5){
            setting.compressionFormat = AudioCompressionFormat.Vorbis;
            setting.loadType = AudioClipLoadType.Streaming;
        }
        else if(clipLength >= 1){
            if(isAllowPlaybackLatency){
                setting.compressionFormat = AudioCompressionFormat.Vorbis;            
            }
            else{
                setting.compressionFormat = AudioCompressionFormat.ADPCM;            
            }
            setting.loadType = AudioClipLoadType.CompressedInMemory;
        }
        else{
            if(isAllowPlaybackLatency){
                setting.loadType = AudioClipLoadType.CompressedInMemory;
            }
            else{
                setting.loadType = AudioClipLoadType.DecompressOnLoad;
            }
            setting.compressionFormat = AudioCompressionFormat.ADPCM;           
        }
        return setting;
    }
}