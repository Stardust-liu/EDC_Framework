using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example{
    public class Window1_V : BaseUI_V<Window1_P>
    {
        public Button openWindow2;
        public Button closeBtn;
        protected override void Start()
        {
            openWindow2.onClick.AddListener(presenter.OpenWindow2);
            closeBtn.onClick.AddListener(presenter.Close);
        }
    }
}