using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseViewControl : IBaseUIControl { }
public class BaseViewControl<T> : BaseUIControl<T>, IBaseViewControl
where T : IBaseUI
{ }