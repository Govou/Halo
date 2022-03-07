using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model;

namespace halobiz_backend.Model
{
    public class Notification
    {
        [Key]
        public long Id { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        [Required]
        public bool IsMailSent { get; set; }= false;
        [Required]
        public string Subject { get; set; }
        public long SenderId { get; set; }
        public UserProfile Sender { get; set; }
        [Required]
        public string Recepients { get; set; }
        public string  Bcc { get; set; }
        public string Cc { get; set; }
        [StringLength(20000)]
        public string Body  { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
