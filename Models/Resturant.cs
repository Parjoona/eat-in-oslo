using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatInOslo.Models
{
    public class Restaurant
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        // Uploadable!
        public string imgurl { get; set; }
        // Use starts from reviews to get average stars
        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<Image> Image { get; set; }        
    }
}