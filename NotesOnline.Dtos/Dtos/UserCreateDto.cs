using System.ComponentModel.DataAnnotations;

namespace NotesOnline.Dtos
{
    /// <summary>
    /// A DTO for creating users
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// The users user name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }


    }
}
