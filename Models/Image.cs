using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace EatInOslo.Models
{
    public class Image
    {
        [Key]
        public int ID { get; set; }
        public string imgurl { get; set; }
        public string description { get; set; }

        [ForeignKey("RestaurantID")]
        public int? RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}