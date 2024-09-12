using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class Window2_V : BaseUI_V
    {
        public CustomClickEffectsBtn closeBtn;

        protected override void Start()
        {
            closeBtn.onClickEvent.AddListener(Hub.Window.CloseWindow);
        }
    }
}