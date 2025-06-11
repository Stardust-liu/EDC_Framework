using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    public class AudioData : BaseGameArchive
    {
        public float soundMainVolume = 1f;
        public float soundBgVolume = 1f;
        public float soundEffectVolume = 1f;
        public float soundDialogueVolume = 1f;

        /// <summary>
        /// 更新主音量
        /// </summary>
        public void UpdtaeSoundMainVolume(float volume){
            soundMainVolume = volume;
        }

        /// <summary>
        /// 更新背景音量
        /// </summary>
        public void UpdateSoundBgVolume(float volume){
            soundBgVolume = volume;
        }

        /// <summary>
        /// 更新音效音量
        /// </summary>
        public void UpdateSoundEffectVolume(float volume){
            soundEffectVolume = volume;
        }

        /// <summary>
        /// 更新对话音量
        /// </summary>
        public void UpdateSoundDialogueVolume(float volume){
            soundDialogueVolume = volume;
        }
    }
}