using System;

namespace HomeQI.ADream.Entities.Framework
{
    public interface IEntityBase<TKey> where TKey : IEquatable<TKey>
    {
        DateTime CreatedTime { get; set; }
        string CretaedUser { get; set; }
        string Description { get; set; }
        DateTime EditedTime { get; set; }
        string EditeUser { get; set; }
        TKey Id { get; set; }
        bool IsDeleted { get; set; }
    }
}