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

        [StringLength(200, MinimumLength=3, ErrorMessage="A description needs 200-3 letters")]
        [Required]
        public string description { get; set; }

        [ForeignKey("RestaurantID")]
        public int? RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}