using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatInOslo.Models
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage="Need to input")]
        public string title { get; set; }
        public string text { get; set; }

        [ForeignKey("RestaurantID")]
        public int? RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey("UserID")]
        public int? UserID { get; set; }
        public virtual User User { get; set; }
    }
}