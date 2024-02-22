using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class EventCenterlExample : MonoBehaviour
    {
        public CustomClickEffectsBtn AddEventBtn;
        public CustomClickEffectsBtn RemoveEventBtn;
        public CustomClickEffectsBtn OnEventBtn;
        public CustomClickEffectsBtn DestroyBtn;
        public MonoEventOwner monoEventOwner;
        public PlainEventOwner plainEventOwner;
        public int value;

        void Start()
        {
            AddEventBtn.onClickEvent.AddListener(ClickAddEventBtn);
            RemoveEventBtn.onClickEvent.AddListener(ClickRemoveEventBtn);
            OnEventBtn.onClickEvent.AddListener(ClickOnEventBtn);
            DestroyBtn.onClickEvent.AddListener(ClickDestroyBtn);
            plainEventOwner = new PlainEventOwner();
        }

        private void ClickAddEventBtn(){
            monoEventOwner.AddEvent();
            plainEventOwner.AddEvent();
        }

        private void ClickRemoveEventBtn(){
            monoEventOwner.RemoveListener();
            plainEventOwner.RemoveListener();
        }

        private void ClickOnEventBtn(){
            Hub.EventCenter.OnEvent_TestEventName(value);        
        }

        private void ClickDestroyBtn(){
            monoEventOwner.Destroy();
            plainEventOwner = null;
        }
    }
}