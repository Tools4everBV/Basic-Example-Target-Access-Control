using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EXAMPLE.API.Access.Control.Data.Models
{
    /// <summary>
    /// A user can have multiple accessKeys. Each AccessKeyAssignment will only have one accessKey like; Card001 or LicensePlate001
    /// <br>
    /// An accessKey will be assigned (granted) to a user.
    /// </br>
    /// </summary>
    public class AccessKeyAssignments
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
        /// The Id of the AccessKeyId that will be assigned.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "AccessKeyId is required")]
        public int AccessKeyId { get; set; }

        /// <summary>
        /// The Id of the user who will be assigned to this accessKey.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
    }
}
