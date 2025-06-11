using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class ObjectPoolExample : MonoBehaviour
    {
        public Button addObject;
        public Button recycleObject;
        public Button destroyObject;
        public Button recycleAllObject;
        public Button destroyAllHideObject;
        public Button initPool;
        public Button destroyPool;
        public Transform objectPoolParent;

        [LabelText("是否预加载对象")]
        public bool isPreloading;

        [LabelText("是否进行批量操作")]
        public bool isBatchOperation;

        [LabelText("销毁时是否销毁激活的元素")]
        public bool isDestroyActiveObje;

        private void Start()
        {
            addObject.onClick.AddListener(ClickAddObject);
            recycleObject.onClick.AddListener(ClickRecycleObject);
            destroyObject.onClick.AddListener(ClickDestroyObject);
            recycleAllObject.onClick.AddListener(ClickRecycleAllObject);
            destroyAllHideObject.onClick.AddListener(ClickDestroyAllHideObject);
            initPool.onClick.AddListener(ClickInitPool);
            destroyPool.onClick.AddListener(ClickDestroyPool);
        }

        /// <summary>
        /// 增加对象
        /// </summary>
        private void ClickAddObject(){
            if(!isBatchOperation){
                TestPoolObj.GetItem();
            }
            else{
                for (int i = 0; i < 5; i++)
                {
                    TestPoolObj.GetItem();            
                }
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        private void ClickRecycleObject(){
            if(!isBatchOperation){
                TestPoolObj.RecycleItem();
            }
            else{
                TestPoolObj.RecycleItem(5);
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>

        private void ClickDestroyObject(){
            if(!isBatchOperation){
                TestPoolObj.DestroyItem(isDestroyActiveObje);
            }
            else{
                TestPoolObj.DestroyItem(5);
            }
        }

        /// <summary>
        /// 回收所有对象
        /// </summary>
        private void ClickRecycleAllObject(){
            TestPoolObj.RecycleAllItem();
        }

        /// <summary>
        /// 销毁所有隐藏对象
        /// </summary>
        private void ClickDestroyAllHideObject(){
            TestPoolObj.DestroyAllHideItem();
        }

        /// <summary>
        /// 初始化对象池
        /// </summary>
        private void ClickInitPool(){
            if(isPreloading){
                TestPoolObj.InitPool(objectPoolParent, 5, true);
            }
            else{
                TestPoolObj.InitPool(objectPoolParent);
            }
        }
        
        /// <summary>
        /// 销毁对象池
        /// </summary>
        private void ClickDestroyPool()
        {
            TestPoolObj.DestroyPool();
        }
    }   
}