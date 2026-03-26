using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{ 
    /// <summary>
    /// 测试事件1
    /// </summary>
    public struct EventExample1{}

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

    /// <summary>
    /// 测试事件序列
    /// </summary>
    public struct EventSequence1{}
    /// <summary>
    /// 测试事件序列
    /// </summary>
    public struct EventSequenceEvent1{}
    /// <summary>
    /// 测试事件序列
    /// </summary>
    public struct EventSequenceEvent2{}

    public class EventCenterExample : MonoBehaviour, 
    IEventCenterLocal, ISendEventLocal, IBindEventLocal, 
    ISendEvent, IBindEvent,
    IEventSequenceDirector, IBindEventSequence, ISendEventSequence
    {
        public Button addEventBtn;
        public Button removeEventBtn;
        public Button postEventBtn;
        public EventCenter EventCenter { get; set; } = new EventCenter();

        private void Start()
        {
            addEventBtn.onClick.AddListener(ClickAddEventBtn);
            removeEventBtn.onClick.AddListener(ClickRemoveEventBtn);
            postEventBtn.onClick.AddListener(ClickPostEventBtn);
            this.AddSequence<EventSequence1, EventExample2>();
            this.SetSequence<EventSequence1>(new List<Type>{typeof(EventSequenceEvent1), typeof(EventSequenceEvent2)});
        }

        private void ClickAddEventBtn()
        {
            this.Register(this);
            this.AddListener<EventExample1>(FunctionA);
            this.AddListener<EventExample2>(FunctionB);
            this.AddListener<EventExample3, EventCenterExample>(Function_Local_A);

            this.AddListener<EventSequence1, EventSequenceEvent1>(FunctionA_Async);
            this.AddListener<EventSequence1, EventSequenceEvent1>(FunctionB_Async);
            this.AddListener<EventSequence1, EventSequenceEvent2>(FunctionC_Async);

            this.AddListener<EventExample3>(FunctionD);
            //↑为了演示触发EventExample3时，由于EventExample3是局部事件，所以不会调用FunctionD
            //如果要触发FunctionD，需要在全局事件方法上调用，如this.SendEvent<EventExample3>();
            Debug.Log("添加监听");
        }
        private void ClickRemoveEventBtn()
        {
            this.RemoveListener<EventExample1>(FunctionA);
            this.RemoveListener<EventExample2>(FunctionB);
            this.RemoveListener<EventExample3, EventCenterExample>(Function_Local_A);
            this.Remove<EventCenterExample>();

            this.RemoveListener<EventSequence1, EventSequenceEvent1>(FunctionA_Async);
            this.RemoveListener<EventSequence1, EventSequenceEvent1>(FunctionB_Async);
            this.RemoveListener<EventSequence1, EventSequenceEvent2>(FunctionC_Async);

            this.RemoveListener<EventExample3>(FunctionD);
            Debug.Log("移除监听");
        }
        private void ClickPostEventBtn()
        {
            this.SendEvent<EventExample1>();
            this.SendEvent(new EventExample2(1));
            this.SendEvent<EventExample3, EventCenterExample>(new EventExample3(2));
            this.SendEventSequence<EventSequence1>();
        }

        private void FunctionA()
        {
            Debug.Log("调用FunctionA");
        }
        
        private void FunctionB(EventExample2  eventExample2)
        {
            Debug.Log($"调用FunctionB，参数{eventExample2.value1}");
        }
        
        private void Function_Local_A(EventExample3 eventExample3)
        {
            Debug.Log($"调用Function_Local_A，参数{eventExample3.value1}");
        }

        private void FunctionD(EventExample3 eventExample3)
        {
            Debug.Log($"调用FunctionD，参数{eventExample3.value1}");
        }

        public async UniTask FunctionA_Async()
        {
            await UniTask.Delay(1000); 
            Debug.Log("FunctionA_Async");
        }
        public async UniTask FunctionB_Async()
        {
            await UniTask.Delay(2000); 
            Debug.Log("FunctionB_Async");
        }
        public async UniTask FunctionC_Async()
        {
            await UniTask.Delay(3000); 
            Debug.Log("FunctionC_Async");
        }
    }    
}

