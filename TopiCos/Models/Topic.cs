using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client.AppConfig;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopiCos.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        public int? RoomId { get; set; }

        [ForeignKey("RoomId")]

        public Room? ARoom { get; set; }

        public List<Comment>? Comments { get; set; }




    }
}

