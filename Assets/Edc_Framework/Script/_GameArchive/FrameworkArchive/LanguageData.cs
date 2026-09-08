using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    [ArchiveKey("LanguageData")]
    public class LanguageData : BaseGameArchive
    {
        public SystemLanguage currentLanguage;

        protected internal override void OnCreateDefaultData()
        {
            currentLanguage = SystemLanguage.ChineseSimplified;
        }

        /// <summary>
        /// 更新语言
        /// </summary>
        public void ChangeLanguage(SystemLanguage languageId){
            currentLanguage = languageId;
            SetDirty();
        }
    }
}
