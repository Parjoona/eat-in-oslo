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

        [StringLength(48, MinimumLength=4, ErrorMessage="Username length needs to be 4-48 symbols")]
        [Required]
        public string name { get; set; }

        [StringLength(48, MinimumLength=4, ErrorMessage="Password length needs to be 4-48 symbols")]
        [Required]
        public string password { get; set; }

        [EmailAddress(ErrorMessage="Write a e-mail adress correctly, example: Admin@EatInOslo.no")]
        [Required]
        public string email { get; set; }

        public virtual ICollection<Review> Review { get; set; }
    }
}