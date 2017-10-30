using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class MessageModel : BaseEntity
    {
        [Required(ErrorMessage = "Message is required")]
        [MinLength(5, ErrorMessage= "Message must be more than 5 characters")]
        public string message {get; set;}
    }
}