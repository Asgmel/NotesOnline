using System.ComponentModel.DataAnnotations;

namespace NotesOnline.Dtos
{
    /// <summary>
    /// A DTO for reading users
    /// </summary>
    public class UserReadDto
    {
        /// <summary>
        /// The users ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The users user name
        /// </summary>
        public string? UserName { get; set; }
    }
}
