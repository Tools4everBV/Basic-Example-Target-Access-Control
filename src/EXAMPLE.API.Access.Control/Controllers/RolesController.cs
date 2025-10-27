using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXAMPLE.API.Access.Control.Data;
using EXAMPLE.API.Access.Control.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace EXAMPLE.API.Access.Control.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")] 
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/roles
        /// <summary>
        /// Get roles
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// Before we can assign a role to a user, we first need to retrieve all available roles. Then, we build out our business rules to, ultimately grant authorizations to users based on information coming from an HR source.
        /// </remarks>
        /// <response code="200"></response>
        [HttpGet(Name = "GetAllRoles")]
        [ProducesResponseType(typeof(List<Role>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole()
        {
            return await _context.Role.ToListAsync();
        }

#if DEBUG

        // POST: api/roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add Role (optional test)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action only for testing this connector, generally we do not dynamically create roles with helloID 
        /// <br></br>
        /// Example:
        ///   
        ///     POST /roles
        ///     {
        ///        "displayName": "Admin"
        ///        
        ///     }
        ///     
        /// </remarks>
        /// <param name="auth">The authorization that will be added.</param>
        /// <response code="201"></response>
        [HttpPost("", Name = "Add")]
        [ProducesResponseType(typeof(Role), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AccessKeyAssignment>> Test_PostRole([FromBody] Role auth)
        {
            _context.Role.Add(auth);
            await _context.SaveChangesAsync();

            return new ObjectResult(auth) { StatusCode = StatusCodes.Status201Created };
        }
#endif
    }
}
