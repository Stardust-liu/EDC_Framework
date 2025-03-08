using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISendQuery{}
public static class ISendQueryExtension{
    public static CallBackType SendQuery<CallBackType>(this ISendQuery sendQuery, BaseQuery<CallBackType> query)
    {
        return ((IQuery<CallBackType>)query).GetResult();
    }
}
