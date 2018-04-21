using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace EatInOslo.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public virtual ICollection<Review> Review { get; set; }
    }
}