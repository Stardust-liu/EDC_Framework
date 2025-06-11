using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using UnityEngine;
public interface IIOCComponent 
{
    void Init();
    void Uninstall();
}

public class BaseIOCComponent<T> : BaseIOCComponent
where T : BaseGameArchive, new()
{
    protected T Data {get; private set;}
    protected override void Init()
    {
        base.Init();
        Data = GameArchive.GetData<T>();
    }

    protected override void Uninstall()
    {
        base.Uninstall();
        Data = null;
        GameArchive.ReleaseData<T>();
    }
}

public abstract class BaseIOCComponent : IIOCComponent
{
    void IIOCComponent.Init(){
        Init();
    }
    void IIOCComponent.Uninstall(){
        Uninstall();
    }
    protected virtual void Init(){}
    protected virtual void Uninstall(){}
}

public class BaseMonoIOCComponent<T> : BaseMonoIOCComponent
where T : BaseGameArchive, new()
{
    protected T Data {get; private set;}
    protected override void Init()
    {
        base.Init();
        Data = GameArchive.GetData<T>();
    }

    protected override void Uninstall()
    {
        base.Uninstall();
        Data = null;
        GameArchive.ReleaseData<T>();
    }
}

public class BaseMonoIOCComponent : MonoBehaviour, IIOCComponent
{
    void IIOCComponent.Init(){
        Init();
    }
    void IIOCComponent.Uninstall(){
        Uninstall();
    }
    protected virtual void Init(){}
    protected virtual void Uninstall(){}
}

