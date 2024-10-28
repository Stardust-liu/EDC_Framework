using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BasePool : MonoBehaviour{
    public virtual void Create(){}//创建对象
    public virtual void Init(){}//初始化对象
    public virtual void Recycle(){}//回收对象
    public abstract void Destroy();//销毁对象
    private static ObjectPoolSetting objectPool;
    protected static ObjectPoolSetting ObjectPool{
        get{
           
            objectPool ??= Hub.Resources.GetScriptableobject<ObjectPoolSetting>(nameof(ObjectPoolSetting));
            return objectPool;
        }
    }

    public static GameObject GetFrameworkPool(string poolName){
        return ObjectPool.GetFrameworkPool(poolName);
    }

    public static GameObject GetPool(string poolName, string sceneName = null){
        return ObjectPool.GetPool(poolName, sceneName);
    }
}

public abstract class BasePoolManager<T> : BasePool where T : BasePool
{
    protected static GameObject prefab;
    protected static Transform parent;
    private static bool isInit;
    private static int createCount;
    private static List<T> hideObject;
    private static List<T> activeObject;
    protected static int HideObjectCount{ get{return hideObject.Count;}}
    protected static int ActiveObjectCount{ get{return activeObject.Count;}}
    protected static int CreateCount{ get{return createCount;}}

    public static void InitPool(GameObject prefabObj, Transform parentTransform = null, int count = 0, bool isPreloading = false){
        if(count != 0){
            hideObject = new List<T>(count);
            activeObject = new List<T>(count);
        }
        else{
            hideObject = new List<T>();
            activeObject = new List<T>();
        }
        prefab = prefabObj;
        parent = parentTransform;
        isInit = true;
        if(isPreloading){
            for (int i = 0; i < count; i++)
            {
                PreloadingObject();
            }
        }
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    public static T GetItem(){
        T item;
        if (hideObject.Count > 0)
        {
            item = hideObject[0];
            activeObject.Add(item);
            hideObject.RemoveAt(0);
            item.Init();
            return item;
        }
        else
        {
            item = Create().GetComponent<T>();
            activeObject.Add(item);
            item.Create();
            item.Init();
            return item;
        }
    }

#region 回收对象相关
    /// <summary>
    /// 回收指定数量对象
    /// </summary>
    public static void RecycleItem(int count){
        count = Mathf.Clamp(count, 0, ActiveObjectCount);
        for (int i = 0; i < count; i++)
        {
            RecycleItem();
        }
    }

    /// <summary>
    /// 回收元素
    /// </summary>
    public static void RecycleItem(){
        T item;
        if(activeObject.Count > 0 ){
            item = activeObject[0];
            if(item != null){
                item.Recycle();
                hideObject.Add(item);
                activeObject.RemoveAt(0);
            }
        }
        else{
            LogManager.Log("激活元素已全部回收");
        }
    }

    /// <summary>
    /// 回收指定对象
    /// </summary>
    public static void RecycleItem(T item = null){
        if(!hideObject.Contains(item)){
            item.Recycle();
            hideObject.Add(item);
            activeObject.Remove(item);
        }
        else{
            LogManager.LogError("指定对象已回收");
        }
    }

    /// <summary>
    /// 回收所有对象
    /// </summary>
    public static void RecycleAllItem(){
        foreach (var item in activeObject)
        {
            item.Recycle();
            hideObject.Add(item);
        }
        activeObject.Clear();
    }
#endregion
#region 销毁对象相关
    /// <summary>
    /// 销毁指定数量的对象
    /// </summary>
    public static void DestroyItem(int count, bool isDestroyActiveObje = false){
        if(!isDestroyActiveObje){
            count = Mathf.Clamp(count, 0, HideObjectCount);
        }
        else{
            count = Mathf.Clamp(count, 0, CreateCount);
        }
        for (int i = 0; i < count; i++)
        {
            DestroyItem(isDestroyActiveObje);
        }
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    public static void DestroyItem(bool isDestroyActiveObje = false){
        T item;
        if(hideObject.Count > 0){
            item = hideObject[0];
            if(item != null){
                hideObject.RemoveAt(0);
                item.Destroy();
            }
        }
        else if(activeObject.Count > 0 && isDestroyActiveObje){
            item = activeObject[0];
            if(item != null){
                activeObject.RemoveAt(0);
                item.Destroy();
            }
        }
        else{
            LogManager.Log("元素已全部销毁");
        }
    }

    /// <summary>
    /// 销毁指定对象
    /// </summary>
    public static void DestroyItem(T item){
        if(hideObject.Contains(item)){
            hideObject.Remove(item);
        }
        else{
            activeObject.Remove(item);
        }
        item.Destroy();
    }

    /// <summary>
    /// 销毁所有未激活对象
    /// </summary>
    public static void DestroyAllHideItem(){
        if(hideObject.Count > 0){
            var endItem = hideObject[0];
            foreach (var item in hideObject)
            {
                item.Destroy();
            }
            hideObject.Clear();
            endItem.Destroy();
        }
        else{
            LogManager.LogError("没有未激活对象");
        }
    }

        
    /// <summary>
    /// 销毁整个池
    /// </summary>
    public static void DestroyPool()
    {
        if(!isInit){
            LogManager.LogError("对象池已销毁");
            return;
        }
        T endItem;
        if(hideObject.Count > 0){
            endItem = hideObject[0];
            hideObject.RemoveAt(0);
        }
        else{
            endItem = activeObject[0];
            activeObject.RemoveAt(0);
        }
        if(endItem != null){
            foreach (var item in hideObject)
            {
                item.Destroy();
            }
            foreach (var item in activeObject)
            {
                item.Destroy();
            }
            prefab = null;
            parent = null;
            hideObject = null;
            activeObject = null;
            isInit = false;
            endItem.Destroy();
        }
        else{
            prefab = null;
            parent = null;
            hideObject = null;
            activeObject = null;
            isInit = false;
            LogManager.LogError("销毁了一个完全未创建过对象的对象池");
        }
    }
#endregion   
    public override void Destroy()
    {
        createCount --;
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// 预加载对象
    /// </summary>
    private static void PreloadingObject(){
        var item = Create().GetComponent<T>();
        item.Create();
        item.Recycle();
        hideObject.Add(item);
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    private static new GameObject Create(){
        createCount ++;
        if(parent != null){
            return GameObject.Instantiate(prefab, parent);

        }
        else{
            return GameObject.Instantiate(prefab);
        }
    }
}
