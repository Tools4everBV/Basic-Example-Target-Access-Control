using System.ComponentModel.DataAnnotations;

namespace EXAMPLE.API.Access.Control.Data.Models
{
    /// <summary>
    /// The AccessKey that are available in the target system. Like; 'Card001' or 'Card002'.
    /// The Permissions on the accessKey are managed due to the Roles on the user object. The permissions assigned to the user object, will be applied to the assigned Accesskeys.
    /// </summary>
    public class AccessKey
    {
        /// <summary>
        /// This is the internal / database Id.
        /// <br>
        /// Typically this value will be set by the application itself.
        /// </br>
        /// </summary>
        
        [Key]
        public int Id { get; internal set; }

        /// <summary>
        /// The DisplayName of the AccessKey
        /// </summary>
        /// <example>Card001</example>
        public string DisplayName { get; set; }


        /// <summary>
        /// The Type of the AccessKey
        /// </summary>
        /// <example>Employee, Visitor or temporarily</example>
        public string? Type { get; set; }


        /// <summary>
        /// The state of the AccessKey
        /// </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }

    }
}
