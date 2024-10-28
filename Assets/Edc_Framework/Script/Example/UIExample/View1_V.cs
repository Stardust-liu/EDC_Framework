using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class View1_V : BaseUI_V<View1_P>
    {
        public Button changeView2;

        protected override void Start()
        {
            changeView2.onClick.AddListener(presenter.ClickChangeView2);
        }
    }
}