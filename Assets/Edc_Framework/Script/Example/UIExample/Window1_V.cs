using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class Window1_V : BaseUI_V
    {
        public CustomClickEffectsBtn openNextWindow;
        public CustomClickEffectsBtn closeBtn;
        protected override void OnActive()
        {
        }

        protected override void OnStart()
        {
            openNextWindow.onClickEvent.AddListener(()=>{Hub.Window.OpenWindow(Window2_C.Instance);});
            closeBtn.onClickEvent.AddListener(Hub.Window.CloseWindow);
        }
    }
}