using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class Window2_V : BaseUI_V
    {
        public CustomClickEffectsBtn closeBtn;
        protected override void OnActive()
        {
        }

        protected override void OnStart()
        {
            closeBtn.onClickEvent.AddListener(Hub.Window.CloseWindow);
        }
    }
}