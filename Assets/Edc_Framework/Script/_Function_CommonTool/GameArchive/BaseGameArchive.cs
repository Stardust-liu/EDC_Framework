using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
namespace ArchiveData{
    public interface IBaseGameArchive{
        void InitData();
    }

    public abstract class BaseGameArchive : IBaseGameArchive
    {
        public bool isInitSave;
        void IBaseGameArchive.InitData(){
            //InitData();
        }

        //protected abstract void InitData();
    }
}

