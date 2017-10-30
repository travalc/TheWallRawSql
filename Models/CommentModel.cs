using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class CommentModel : BaseEntity
    {
        [Required(ErrorMessage = "Comment is required")]
        [MinLength(5, ErrorMessage= "Comment must be more than 5 characters")]
        public string comment {get; set;}
        [Required]
        public int Messages_id {get; set;}
    }
}