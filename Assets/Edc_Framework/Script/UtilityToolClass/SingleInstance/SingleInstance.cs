using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstance<T> where T : class, new()
{
    private static T instance;
    public static T Instance{
        get {
            instance ??= new T();
            return instance;
        }
    }

    public virtual void Destroy(){
        instance = null;
    }
}
