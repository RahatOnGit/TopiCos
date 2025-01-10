using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopiCos.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ValidateNever]
        public string? Description { get; set; }

        [ValidateNever]
        public string RoomId { get; set; }

        [ValidateNever]
        public DateTime Created { get; set; }

        [ValidateNever]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        public List<RoomDetails>? RoomDetails { get; set; }

        public List<Topic>? Topics { get; set; }



    }
}
