using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IQuery<CallBackType>{ 
    CallBackType GetResult();
}
public abstract class BaseQuery<CallBackType> : IQuery<CallBackType>, ISendQuery
{
    CallBackType IQuery<CallBackType>.GetResult(){
        return GetResult();
    }
    protected abstract CallBackType GetResult();
}
