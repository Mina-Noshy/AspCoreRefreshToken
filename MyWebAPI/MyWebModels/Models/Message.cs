using MyWebModels.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebModels.Models
{
    public class Message
    { 
        [Key]
        public decimal Id { get; set; }
        public int ChatId { get; set; }
        public string SenderId { get; set; }

        [MaxLength(200)]
        [Required]
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsSeen { get; set; } = false;

        [ForeignKey("SenderId")]
        public virtual AppUser GetSender { get; set; }

        [ForeignKey("ChatId")]
        public virtual Chat GetChat { get; set; }
    }
}
