using System.Collections;
using System.Collections.Generic;

namespace ArchiveData{
    public class RedDotData : BaseGameArchive
    {
        public Dictionary<RedDotLeafNode, bool> leafRedDotState = new();
        public Dictionary<RedDotNode, bool> nonLeafRedDotState = new();

        /// <summary>
        /// 更新起点或分支节点状态
        /// </summary>
        public void UpdateRedDotState(RedDotNode redDotNode, bool isActive){
            nonLeafRedDotState[redDotNode] = isActive;
            //不需要调用保存方法，数据会通过后续调用ActiveRedDot或DisableRedDot方法保存
        }

        /// <summary>
        /// 激活红点
        /// </summary>
        public void ActiveRedDot(RedDotLeafNode redDotLeafNode){
            leafRedDotState[redDotLeafNode] = true;
        }
        
        /// <summary>
        /// 隐藏红点
        /// </summary>
        public void DisableRedDot(RedDotLeafNode redDotLeafNode){
            leafRedDotState[redDotLeafNode] = false;
        }
    }
}
