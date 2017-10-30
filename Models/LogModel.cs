using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class LogModel : BaseEntity
    {
        [Required]
        public string email {get; set;}
        [Required]
        [DataType(DataType.Password)]
        public string password {get; set;}
    }
}