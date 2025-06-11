using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{ 
    /// <summary>
    /// 测试事件1
    /// </summary>
    public struct EventExample1
    {

    }

    /// <summary>
    /// 测试事件2
    /// </summary>
    public struct EventExample2
    {
        public int value1;

        public EventExample2(int _value1)
        {
            this.value1 = _value1;
        }
    }

    /// <summary>
    /// 测试事件3
    /// </summary>
    public struct EventExample3
    {
        public int value1;

        public EventExample3(int _value1)
        {
            this.value1 = _value1;
        }
    }

    public class EventCenterExample : MonoBehaviour, ISendEvent, IBindEvent, IEventCenter
    {
        public Button addEventBtn;
        public Button removeEventBtn;
        public Button postEventBtn;
        public GameObject autoBindEventObj;
        public EventCenter EventCenter { get; set; } = new EventCenter();

        private void Start()
        {
            addEventBtn.onClick.AddListener(ClickAddEventBtn);
            removeEventBtn.onClick.AddListener(ClickRemoveEventBtn);
            postEventBtn.onClick.AddListener(ClickPostEventBtn);
        }

        private void ClickAddEventBtn()
        {
            EventHub.Register(this);
            this.AddListener<EventExample1>(FunctionA);
            this.AddListener<EventExample2>(FunctionB);
            this.AddListener<EventExample3, EventCenterExample>(FunctionC);
            this.AddListener<EventExample3>(FunctionD);
            //↑只是为了演示触发EventExample3时，由于EventExample3是独立事件，所以不会调用FunctionD
        }
        private void ClickRemoveEventBtn()
        {
            this.RemoveListener<EventExample1>(FunctionA);
            this.RemoveListener<EventExample2>(FunctionB);
            this.RemoveListener<EventExample3, EventCenterExample>(FunctionC);
            this.AddListener<EventExample3>(FunctionD);
            EventHub.Remove<EventCenterExample>();
        }
        private void ClickPostEventBtn()
        {
            this.SendEvent<EventExample1>();
            this.SendEvent(new EventExample2(1));
            this.SendEvent<EventExample3, EventCenterExample>(new EventExample3(2));
        }

        private void FunctionA()
        {
            Debug.Log("调用FunctionA");
        }
        
        private void FunctionB(EventExample2  eventExample2)
        {
            Debug.Log($"调用FunctionB，参数{eventExample2.value1}");
        }
        
        private void FunctionC(EventExample3 eventExample3)
        {
            Debug.Log($"调用FunctionC，参数{eventExample3.value1}");
        }

        private void FunctionD(EventExample3 eventExample3)
        {
            Debug.Log($"调用FunctionD，参数{eventExample3.value1}");
        }
    }    
}

