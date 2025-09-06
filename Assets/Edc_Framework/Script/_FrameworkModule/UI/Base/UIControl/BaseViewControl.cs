using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseViewControl : IBaseUIControl { }
public class BaseViewControl<T> : BaseUIControl<T>, IBaseViewControl
where T : IBaseUI
{ }

public class BaseViewControl<panel_2D, panel_3D> : BaseUIControl<panel_2D, panel_3D>, IBaseViewControl
where panel_2D : IBaseUI
where panel_3D : IBaseUI
{ }