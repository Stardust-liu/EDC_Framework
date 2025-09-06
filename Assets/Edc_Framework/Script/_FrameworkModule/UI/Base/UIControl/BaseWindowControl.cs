using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseWindowControl : IBaseUIControl { }

public class BaseWindowControl<T> : BaseUIControl<T>, IBaseWindowControl
where T : IBaseUI
{ }

public class BaseWindowControl<panel_2D, panel_3D> : BaseUIControl<panel_2D, panel_3D>, IBaseWindowControl
where panel_2D : IBaseUI
where panel_3D : IBaseUI
{ }
