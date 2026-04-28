using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePersistentViewControl : IBaseUIControl { }
public class BasePersistentViewControl<T> : BaseUIControl<T>, IBasePersistentViewControl
where T : IBaseUI
{ }
