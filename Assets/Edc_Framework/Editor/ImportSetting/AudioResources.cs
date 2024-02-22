using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AudioResources : AssetPostprocessor
{
    /// <summary>
    /// 是否允许有一定的播放延迟
    /// </summary>
    private bool isAllowPlaybackLatency;

    /// <summary>
    /// 是否是高质量音频
    /// </summary>
    private bool isHighQuality;
    public string dialogueAudioPath = "Assets/Game/Sources/Resources/Dialogue";//默认对话模块的素材都是高质量的
    
    private void OnPostprocessAudio(AudioClip clip)
    {
        int cutEnd = assetPath.LastIndexOf('/') + 1;
        string assetName = assetPath.Substring(cutEnd, assetPath.LastIndexOf('.') - cutEnd);

        string cutResult = assetPath.Substring(0, assetPath.LastIndexOf('/'));
        cutEnd = cutResult.LastIndexOf('/')+1;
        string playLatencyData = assetPath.Substring(cutEnd, cutResult.Length - cutEnd);

        cutResult = cutResult.Substring(0, cutResult.LastIndexOf('/'));
        cutEnd = cutResult.LastIndexOf('/')+1;
        string qualityData = assetPath.Substring(cutEnd, cutResult.Length - cutEnd);

        isAllowPlaybackLatency = (playLatencyData == "AllowPlaybackLatency")? true : false;
        isHighQuality = (qualityData == "HighQuality")? true: false;
        isHighQuality = !isHighQuality? assetPath.StartsWith(dialogueAudioPath) : true;

        
        AudioImporter audioImporter = (AudioImporter)assetImporter;
        audioImporter.forceToMono = true;
        AudioImporterSampleSettings Setting = SetLoadTypeAndFormatType(clip.length);
        Setting.sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate;
        Setting.sampleRateOverride = (uint)(isHighQuality ? 44100: 22050);

        audioImporter.defaultSampleSettings = Setting;
#if UNITY_STANDALONE
        audioImporter.SetOverrideSampleSettings("Standalone", Setting);
#elif UNITY_ANDROID
        audioImporter.SetOverrideSampleSettings("Android", Setting);
#elif UNITY_IOS
        audioImporter.SetOverrideSampleSettings("iOS", Setting);
#endif
    }

    /// <summary>
    /// 设置压缩格式和加载方式
    /// </summary>
    private AudioImporterSampleSettings SetLoadTypeAndFormatType(float clipLength)
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
