using System;
using System.Collections;
using System.Collections.Generic;

namespace ArchiveData{
    public class RedDotNodeData
    {
        public bool isActive;
        public int activeCount;
        public RedDotNodeData(bool _isActive, int _activeCount)
        {
            isActive = _isActive;
            activeCount = _activeCount;
        }
    }

    [ArchiveKey("RedDotData")]
    public class RedDotData : BaseGameArchive
    {
        public Dictionary<RedDotLeafNode, bool> leafRedDotState;
        public Dictionary<RedDotNode, RedDotNodeData> nonLeafRedDotState;

        protected internal override void OnCreateDefaultData()
        {
            leafRedDotState = new();
            nonLeafRedDotState = new();
            EnsureDefaultRedDotState();
        }

        protected internal override void OnAfterLoad()
        {
            base.OnAfterLoad();
            leafRedDotState ??= new();
            nonLeafRedDotState ??= new();
            if (EnsureDefaultRedDotState())
            {
                SetDirty();
            }
        }

        private bool EnsureDefaultRedDotState()
        {
            var isChanged = false;
            var redDotLeafNodeArray = Enum.GetValues(typeof(RedDotLeafNode));
            var redDotNodeArray = Enum.GetValues(typeof(RedDotNode));
            foreach (RedDotLeafNode item in redDotLeafNodeArray)
            {
                if (leafRedDotState.ContainsKey(item))
                {
                    continue;
                }

                leafRedDotState.Add(item, false);
                isChanged = true;
            }

            foreach (RedDotNode item in redDotNodeArray)
            {
                if (nonLeafRedDotState.ContainsKey(item))
                {
                    continue;
                }

                nonLeafRedDotState.Add(item, new RedDotNodeData(false, 0));
                isChanged = true;
            }

            return isChanged;
        }

        /// <summary>
        /// 更新根节点或分支节点状态
        /// </summary>
        public void UpdateRedDotState(RedDotNode redDotNode, bool isActive, int activeCount){
            if (nonLeafRedDotState.TryGetValue(redDotNode, out var data))
            {
                data.isActive = isActive;
                data.activeCount = activeCount;
            }
            else
            {
                nonLeafRedDotState.Add(redDotNode, new RedDotNodeData(isActive, activeCount));
            }
            SetDirty();
        }

        /// <summary>
        /// 更新叶子节点状态
        /// </summary>
        public void UpdateleafRedDotState(RedDotLeafNode redDotLeafNode, bool isActive)
        {
            leafRedDotState[redDotLeafNode] = isActive;
            SetDirty();
        }
    }
}
