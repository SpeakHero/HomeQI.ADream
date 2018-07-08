using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    public class UserOrg : EntityBase
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string OrganizationId { get; set; }
        public User User { get; set; }
        public Organization Organization { get; set; }
    }
}
