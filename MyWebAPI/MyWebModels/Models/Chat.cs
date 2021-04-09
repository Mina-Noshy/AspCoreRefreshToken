using MyWebModels.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public string UserId_1 { get; set; }
        public string UserId_2 { get; set; }

        public virtual ICollection<Message> GetMessages { get; set; }

        [ForeignKey("UserId_1")]
        public virtual AppUser GetUser_1 { get; set; }

        [ForeignKey("UserId_2")]
        public virtual AppUser GetUser_2 { get; set; }

    }
}
