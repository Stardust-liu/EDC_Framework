using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    [ArchiveKey("AudioData")]
    public class AudioData : BaseGameArchive
    {
        public float soundMainVolume;
        public float soundBgVolume;
        public float soundEffectVolume;
        public float soundDialogueVolume;

        protected internal override void OnCreateDefaultData()
        {
            soundMainVolume = 1f;
            soundBgVolume = 1f;
            soundEffectVolume = 1f;
            soundDialogueVolume = 1f;
        }

        /// <summary>
        /// 更新主音量
        /// </summary>
        public void UpdtaeSoundMainVolume(float volume){
            soundMainVolume = volume;
            SetDirty();
        }

        /// <summary>
        /// 更新背景音量
        /// </summary>
        public void UpdateSoundBgVolume(float volume){
            soundBgVolume = volume;
            SetDirty();
        }

        /// <summary>
        /// 更新音效音量
        /// </summary>
        public void UpdateSoundEffectVolume(float volume){
            soundEffectVolume = volume;
            SetDirty();
        }

        /// <summary>
        /// 更新对话音量
        /// </summary>
        public void UpdateSoundDialogueVolume(float volume){
            soundDialogueVolume = volume;
            SetDirty();
        }
    }
}
