using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    [Table("rtoken")]
    public class RToken : EntityBase
    {

        public string ClientId { get; set; }

        public string RefreshToken { get; set; }

        public int IsStop { get; set; }
    }
}
