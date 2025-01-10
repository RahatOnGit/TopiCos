using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopiCos.Models
{
    public class RoomDetails
    {
        public int Id { get; set; }

        public int? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        public int? MemberTypeId { get; set; }
        [ForeignKey("MemberTypeId")]
        public MemberType? Member { get; set; }
    }
}
