using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParrotnestServer.Models
{
    public enum UserRelationType
    {
        Blocked = 1,
        Ignored = 2
    }

    public class UserRelation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequesterId { get; set; }

        [ForeignKey("RequesterId")]
        public User? Requester { get; set; }

        [Required]
        public int TargetUserId { get; set; }

        [ForeignKey("TargetUserId")]
        public User? TargetUser { get; set; }

        [Required]
        public UserRelationType RelationType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
