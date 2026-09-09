using System;
using Newtonsoft.Json;
namespace ArchiveData{
    public enum ArchiveDomain
    {
        Global,
        Slot
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ArchiveKeyAttribute : Attribute
    {
        public string Key { get; }
        public ArchiveDomain Domain { get; }

        public ArchiveKeyAttribute(string key, ArchiveDomain domain = ArchiveDomain.Global)
        {
            Key = key;
            Domain = domain;
        }
    }

    public abstract class BaseGameArchive
    {
        [JsonIgnore]
        public Type type;

        [JsonIgnore]
        public ArchiveDomain archiveDomain;

        /// <summary>
        /// 标记数据已经被修改。
        /// </summary>
        protected void SetDirty()
        {
            GameArchive.AddDirtyType(archiveDomain, type);
        }

        /// <summary>
        /// 保存或读取完成后清理修改标记。
        /// </summary>
        internal void ClearDirty()
        {
            GameArchive.RemoveDirtyType(archiveDomain, type);
        }

        internal void SetInfo()
        {
            type = GetType();
            archiveDomain = (Attribute.GetCustomAttribute(type, typeof(ArchiveKeyAttribute)) as ArchiveKeyAttribute).Domain;
        }

        /// <summary>
        /// 没有找到存档文件，创建默认数据时调用。
        /// </summary>
        protected internal abstract void OnCreateDefaultData();
        
        /// <summary>
        /// 从存档文件读取数据后调用。
        /// </summary>
        protected internal virtual void OnAfterLoad() {}

        /// <summary>
        /// 保存到文件前调用。
        /// </summary>
        protected internal virtual void OnBeforeSave() {}
    }
}
