using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class Window2_V : BaseUI_V<Window2_P>
    {
        public Button closeBtn;

        protected override void Start()
        {
            closeBtn.onClick.AddListener(presenter.Close);
        }
    }
}