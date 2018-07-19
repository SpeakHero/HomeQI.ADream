using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Entities.Framework
{
    public abstract class EntityBase : EntityBase<string>
    {
        public override string Id => Guid.NewGuid().ToString();
    }
    public abstract class EntityBase<TKey> : IEntityBase<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        [Display(Name = "主键")]
        public virtual TKey Id { get; set; }
        [Display(Name = "创建时间")]
        public virtual DateTimeOffset CreatedTime { get; set; } = DateTime.Now;
        [Timestamp]
        [Display(Name = "最后修改时间")]
        public virtual DateTimeOffset EditedTime { get; set; } = DateTime.Now;
        [StringLength(50)]
        [Display(Name = "创建人")]

        public virtual string CretaedUser { get; set; } = "";
        [StringLength(50)]
        [Display(Name = "最后编辑人员")]

        public virtual string EditeUser { get; set; } = "";
        [Display(Name = "备注说明")]
        public virtual string Description { get; set; } = "";
        public virtual bool IsDeleted { get; set; } = false;
    }
}
