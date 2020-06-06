using System;
using System.ComponentModel.DataAnnotations;

namespace account_service.Models
{
    public class FollowModel
    {
        [Required] public Guid id { get; set; }
        [Required] public Guid idToFollow { get; set; }
    }
}