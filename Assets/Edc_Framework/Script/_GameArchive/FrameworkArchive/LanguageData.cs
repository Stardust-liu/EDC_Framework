using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchiveData{
    public class LanguageData : BaseGameArchive
    {
        public SystemLanguage currentLanguage = SystemLanguage.ChineseSimplified;

        /// <summary>
        /// 更新语言
        /// </summary>
        public void ChangeLanguage(SystemLanguage languageId){
            currentLanguage = languageId;
        }
    }
}
