using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CommonColorsSetting : SingleInstance<CommonColorsSetting>
{
    [ColorPalette("UI Color")]
    public Color UI;

    [ColorPalette("Scene Color")]
    public Color Scene;
}
