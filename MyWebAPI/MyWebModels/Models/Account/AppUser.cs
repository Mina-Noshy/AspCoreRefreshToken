using Microsoft.AspNetCore.Identity;
using MyWebModels.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Models.Account
{
    public class AppUser : IdentityUser<string>
    {
        public int CityId { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[a-zA-Z0-9\u0600-\u06FF ]+")]
        public string FirstName { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[a-zA-Z0-9\u0600-\u06FF ]+")]
        public string LastName { get; set; }

        public UserTypes UserType { get; set; } = UserTypes.Farmer;

        public UserRanks UserRank { get; set; } = UserRanks.Normal;

        public DateTime ExpireAt { get; set; } = DateTime.UtcNow;

        public string PictureURL { get; set; }

        public List<RefreshTokenVM> RefreshTokens { get; set; }

        [ForeignKey("CityId")]
        public virtual City GetCity { get; set; }

        public virtual ICollection<Chat> GetChates_1 { get; set; }
        public virtual ICollection<Chat> GetChates_2 { get; set; }
        public virtual ICollection<Message> GetMessages { get; set; }
    }

    public enum UserTypes
    {
        Company,
        Exporter,
        Trader,
        Farmer,
        Shop
    }

    public enum UserRanks
    {
        Admin,
        Excellence,
        VeryGood,
        Good,
        Normal
    }

}
