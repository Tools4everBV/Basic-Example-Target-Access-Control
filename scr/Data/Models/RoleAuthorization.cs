using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EXAMPLE.API.Access.Control.Data.Models
{
    /// <summary>
    /// A user can have multiple authorizations. Each authorization will only have one role Like; 'Cleaner', 'Security' or 'Employee'.
    /// <br>
    /// A user will be assigned (granted) to an authorization.
    /// </br>
    /// </summary>
    public class RoleAuthorization
    {
        /// <summary>
        /// This is the internal / database Id.
        /// <br>
        /// Typically this value will be set by the application itself.
        /// </br>
        /// </summary>
        [Key]
        [JsonIgnore]
        public int Id { get; internal set; }

        /// <summary>
        /// The Id of the role that will be assigned.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }

        /// <summary>
        /// The Id of the user who will be assigned to this authorization.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
    }
}
