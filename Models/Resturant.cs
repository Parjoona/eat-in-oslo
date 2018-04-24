using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatInOslo.Models
{
    public class Restaurant
    {
        [Key]
        public int ID { get; set; }

        [StringLength(30, MinimumLength=2, ErrorMessage="Resturant name needs to be 2-30 letters")]
        [Required]
        public string name { get; set; }

        [StringLength(15, MinimumLength=3, ErrorMessage="Type needs to be between 15-3 letters")]
        [Required]
        public string type { get; set; }
        
        [StringLength(250, MinimumLength=15, ErrorMessage="Message needs to be between 250-15 letters")]
        [Required]
        public string description { get; set; }
        // Uploadable!
        public string imgurl { get; set; }
        // Use starts from reviews to get average stars
        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<Image> Image { get; set; }        
    }
}