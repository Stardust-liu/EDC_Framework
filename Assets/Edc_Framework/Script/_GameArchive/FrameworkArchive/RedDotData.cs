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

    public class RedDotData : BaseGameArchive
    {
        public Dictionary<RedDotLeafNode, bool> leafRedDotState = new();
        public Dictionary<RedDotNode, RedDotNodeData> nonLeafRedDotState = new();

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
        }

        /// <summary>
        /// 更新叶子节点状态
        /// </summary>
        public void UpdateleafRedDotState(RedDotLeafNode redDotLeafNode, bool isActive)
        {
            leafRedDotState[redDotLeafNode] = isActive;
        }
    }
}
