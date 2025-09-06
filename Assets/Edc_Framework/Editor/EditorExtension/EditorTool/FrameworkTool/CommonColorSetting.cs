using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CommonColorSetting : SingleInstance<CommonColorSetting>
{
    [ColorPalette("UI Color")]
    public Color UI;
}
