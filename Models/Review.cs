using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatInOslo.Models
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        [StringLength(25, MinimumLength=2, ErrorMessage="Title length needs to be between 2 and 25 letters")]
        [Required]
        public string title { get; set; }
        
        [StringLength(250, MinimumLength=12, ErrorMessage="A message can be between 250 and 12 letters")]
        [Required]
        public string text { get; set; }

        [ForeignKey("RestaurantID")]
        public int? RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey("UserID")]
        public int? UserID { get; set; }
        public virtual User User { get; set; }
    }
}