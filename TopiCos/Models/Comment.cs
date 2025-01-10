using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopiCos.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string CommentData { get; set; }

        public DateTime Created { get; set; }

        public int? TopicId { get; set; }

        [ForeignKey("TopicId")]
        public Topic? ATopic { get; set; }


        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }
}



