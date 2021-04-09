using MyWebModels.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public int CountryId { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[a-zA-Z0-9 ]+")]
        public string EnName { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[0-9\u0600-\u06FF ]+")]
        public string ArName { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country GetCountry { get; set; }

        public virtual ICollection<AppUser> GetUsers { get; set; }
    }
}
