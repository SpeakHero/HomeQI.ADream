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
        public virtual DateTime CreatedTime { get; set; } = DateTime.Now;
        [Timestamp]
        [Display(Name = "最后修改时间")]
        public virtual DateTime EditedTime { get; set; } = DateTime.Now;
        [StringLength(50)]
        [Display(Name = "创建人")]
        [Required]
        public virtual string CretaedUser { get; set; } = string.Empty;
        [StringLength(50)]
        [Display(Name = "最后编辑人员")]
        [Required]
        public virtual string EditeUser { get; set; } = string.Empty;
        [Display(Name = "备注说明")]
        [Required]
        public virtual string Description { get; set; } = string.Empty;
        public virtual bool IsDeleted { get; set; } = false;
    }
}
