using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePersistentViewControl : IBaseUIControl { }
public class BasePersistentViewControl<T> : BaseUIControl<T>, IBasePersistentViewControl
where T : IBaseUI
{ }

public class BasePersistentViewControl<panel_2D, panel_3D> : BaseUIControl<panel_2D, panel_3D>, IBasePersistentViewControl
where panel_2D : IBaseUI
where panel_3D : IBaseUI
{ }
