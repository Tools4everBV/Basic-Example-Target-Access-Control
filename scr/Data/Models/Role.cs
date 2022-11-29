namespace EXAMPLE.API.Access.Control.Data.Models
{
    /// <summary>
    /// The roles that are available in the target system. Like; 'Cleaner', 'Security' or 'Employee'.
    /// A Role of an Access Control System usual contains a set of doors (possible with timeslots) that the user allowed to open.
    /// This implemntation assumes the roles are granted to the user, and the accessKey are assgined to the user. 
    /// The granted roles are applied to the accessKeys assigned to a user.
    ///
    /// </summary>
    public class Role
    {
        /// <summary>
        /// This is the internal / database Id.
        /// <br>
        /// Typically this value will be set by the application itself.
        /// </br>
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The DisplayName of the role
        /// </summary>
        /// <example>Admin</example>
        public string DisplayName { get; set; }
    }
}
