using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IIOCComponent 
{
    void Init();
}

public abstract class BaseIOCComponent : IIOCComponent
{
    void IIOCComponent.Init(){
        Init();
    }
    protected abstract void Init();
}

public class BaseMonoIOCComponent : MonoBehaviour, IIOCComponent
{
    void IIOCComponent.Init(){
        Init();
    }
    protected virtual void Init(){}
}

