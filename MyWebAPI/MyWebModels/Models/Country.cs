using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[a-zA-Z0-9 ]+")]
        public string EnName { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[0-9\u0600-\u06FF ]+")]
        public string ArName { get; set; }

        [MaxLength(3)]
        public string Code { get; set; }

        [MaxLength(3)]
        public string Language { get; set; }


        public virtual ICollection<City> GetCities { get; set; }
    }
}
