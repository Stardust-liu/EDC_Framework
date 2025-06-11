using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{ 
    public class AutoBindEventExample : MonoBehaviour, IAutoBindEvent
    {
        private void Start()
        {
            this.AddListener_StartDestroy<EventExample1>(FunctionE, this.gameObject);
        }

        private void FunctionE()
        {
            Debug.Log("调用FunctionE");
        }
    }   
}
