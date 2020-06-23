using System.ComponentModel.DataAnnotations;

namespace account_service.Models
{
    public class UpdateModel
    {
        [Required] public string Username { get; set; }
    }
}