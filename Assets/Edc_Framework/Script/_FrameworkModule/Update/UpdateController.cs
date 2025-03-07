using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFixedUpdate{
    void OnFixedUpdate();
}

public interface IUpdate{
    void OnUpdate();
}

public interface ILateUpdate{
    void OnLateUpdate();
}

public class UpdateController : BaseMonoIOCComponent
{
    private static readonly List<IFixedUpdate> fixedUpdates = new List<IFixedUpdate>();
    private static readonly List<IUpdate> updates = new List<IUpdate>();
    private static readonly List<ILateUpdate> ILateUpdates = new List<ILateUpdate>();
    private static int fixedUpdatesCount;
    private static int updatesCounnt;
    private static int lateUpdateCount;

    private void FixedUpdate(){
        for (int i = 0; i < fixedUpdatesCount; i++)
        {
            fixedUpdates[i].OnFixedUpdate();
        }
    }

    private void Update(){
        for (int i = 0; i < updatesCounnt; i++)
        {
            updates[i].OnUpdate();
        }
    }

    private void LateUpdate(){
        for (int i = 0; i < lateUpdateCount; i++)
        {
            ILateUpdates[i].OnLateUpdate();
        }
    }

    public void AddFixedUpdate(IFixedUpdate fixedUpdate){
        if(!fixedUpdates.Contains(fixedUpdate)){
            fixedUpdates.Add(fixedUpdate);
            fixedUpdatesCount++;
        }
        else{
            LogManager.LogError("重复添加了一个正在FixedUpdate的对象");
        }
    }

    public void RemoveFixedUpdate(IFixedUpdate fixedUpdate){
        if(fixedUpdates.Contains(fixedUpdate)){
            fixedUpdates.Remove(fixedUpdate);
            fixedUpdatesCount--;
        }
    }


    public void AddUpdate(IUpdate update){
        if(!updates.Contains(update)){
            updates.Add(update);
            updatesCounnt++;
        }
        else{
            LogManager.LogError("重复添加了一个正在Update的对象");
        }
    }

    public void RemoveUpdate(IUpdate update){
        if(updates.Contains(update)){
            updates.Remove(update);
            updatesCounnt--;
        }
    }


    public void AddLateUpdate(ILateUpdate lateUpdate){
        if(!ILateUpdates.Contains(lateUpdate)){
            ILateUpdates.Add(lateUpdate);
            lateUpdateCount++;
        }
        else{
            LogManager.LogError("重复添加了一个正在LateUpdates的对象");
        }
    }

    public void RemoveLateUpdate(ILateUpdate lateUpdate){
        if(ILateUpdates.Contains(lateUpdate)){
            ILateUpdates.Remove(lateUpdate);
            lateUpdateCount--;
        }
    }
}
