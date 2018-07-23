using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimpleEntity
    {
        /// <summary>
        /// 
        /// </summary>
        String Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime EditedTime { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SimpleEntity : ISimpleEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime EditedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public String Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public SimpleEntity()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public SimpleEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="timespan"></param>
        public SimpleEntity(string id, string name, DateTime timespan)
        {
            Id = id;
            Name = name;
            EditedTime = timespan;
        }
    }
}
