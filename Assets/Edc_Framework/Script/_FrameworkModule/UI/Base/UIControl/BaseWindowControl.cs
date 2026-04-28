using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseWindowControl : IBaseUIControl { }

public class BaseWindowControl<T> : BaseUIControl<T>, IBaseWindowControl
where T : IBaseUI
{ }
