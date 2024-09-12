using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomizeUI{

    [Serializable]
    public class EventBase{
        public UnityEngine.Object target;
        public UnityEngine.Object Script;
        public UnityEngine.Object ParamObj;
        public string MethodName = null;
    }

    [Serializable]
    public class CallEvent : EventBase{
        public string ParemType;
    }

    [Serializable]
    public class DataEvent : EventBase{
        public enum ConstDataType{
            None,
            Customer,
        }

        public ConstDataType DataType = ConstDataType.None;
    }

    [Serializable]
    public class MsgOption{
        public CallEvent callEvent;
        public DataEvent dataEvent;

        public MsgOption(){
            
        }
    }
}

