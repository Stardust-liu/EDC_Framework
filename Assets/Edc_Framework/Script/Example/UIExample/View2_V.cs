using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example{
    public class View2_V : BaseUI_V<View2_P>
    {
        public Button changeView3;

        protected override void Start()
        {
            changeView3.onClick.AddListener(presenter.ClickChangeView3);
        }
    }
}