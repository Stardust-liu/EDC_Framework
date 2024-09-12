using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    public class LanguageData : BaseGameArchive<LanguageData>
    {
        public LanguageId currentLanguage;

        /// <summary>
        /// 更新语言
        /// </summary>
        public void ChangeLanguage(LanguageId languageId){
            currentLanguage = languageId;
            SaveDataNow();
        }
    }
}
