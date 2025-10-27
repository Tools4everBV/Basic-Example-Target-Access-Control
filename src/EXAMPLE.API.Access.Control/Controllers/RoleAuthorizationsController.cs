
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXAMPLE.API.Access.Control.Data;
using EXAMPLE.API.Access.Control.Data.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace EXAMPLE.API.Access.Control.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class RoleAuthorizationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleAuthorizationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/roleauthorization
        /// <summary>
        /// Get all authorizations for all users
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We need to retrieve information about all authorizations to support our import entitlement feature, enable reconciliation, and ultimately ensure proper governance.
        /// </remarks>
        /// <returns>List of all authorizations</returns>
        /// <response code="200">Returns the list of all authorizations</response>
        /// <response code="401">Authentication required or failed.</response>
        [HttpGet("", Name = "GetAllRoleAuthorizations")]
        [ProducesResponseType(typeof(List<RoleAuthorization>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<RoleAuthorization>>> GetRoleAuthorization()
        {
            List<RoleAuthorization> authorizations = await _context.RoleAuthorization.ToListAsync();
            return Ok(authorizations);
        }

        // GET: api/roleauthorization/user/{userId}
        /// <summary>
        /// Get all authorizations for a specific user
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This returns all authorizations currently assigned to the given user.
        /// <br></br>
        /// Useful when validating which permissions/roles the user has at a given moment.
        /// </remarks>
        /// <response code="200">Returns a list of authorizations</response>
        /// <response code="404">If the user has no authorizations</response>
        /// <response code="401">Authentication required or failed.</response>
        [HttpGet("{userId:int}", Name = "GetRoleAuthorizationsByUserId")]
        [ProducesResponseType(typeof(List<RoleAuthorization>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Authorization>>> GetRoleAuthorizationsByUserId(int userId)
        {
            var userExists = await _context.User.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound($"User with id {userId} not found.");
            }

            var authorizations = await _context.RoleAuthorization.Where(a => a.UserId == userId).ToListAsync();
            return Ok(authorizations);
        }

        // POST: api/roleauthorization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add authorization
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action when an authorization is granted to a user. Since we do not store the result in HelloID, this action does not require a response
        /// <br></br>
        /// Example:
        ///   
        ///     POST /roleauthorization
        ///     {
        ///        "roleId": 1,
        ///        "userId": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="auth">The authorization that will be added.</param>
        /// <response code="201"></response>
        [HttpPost("", Name = "AddRoleAuthorization")]
        [ProducesResponseType(typeof(RoleAuthorization), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<RoleAuthorization>> PostRoleAuthorization([FromBody] RoleAuthorization auth)
        {
            _context.RoleAuthorization.Add(auth);
            await _context.SaveChangesAsync();

            return new ObjectResult(auth) { StatusCode = StatusCodes.Status201Created };
        }
        // DELETE: api/roleauthorization/:id
        /// <summary>
        /// Delete authorization (by Id)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action when an authorization is revoked from a user. This action does not require a response. A [204 No Content] is sufficient.
        /// </remarks>
        /// <param name="auth">The authorization that will be removed.</param>
        /// <response code="204">The authorization was successfully removed.</response>
        /// <response code="404">No authorization with the specified Id was found.</response>
        /// <response code="401">Authentication required or failed.</response>
        [HttpDelete("{id}", Name = "DeleteAuthorization")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRoleAuthorization(int id)
        {
            var auth = await _context.RoleAuthorization.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            _context.RoleAuthorization.Remove(auth);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/roleauthorization/&userid=:id;&roleid=:id
        /// <summary>
        /// Delete authorization
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action when an authorization is revoked from a user. This action does not require a response. A [204 No Content] is sufficient.
        /// </remarks>
        /// <param name="auth">The authorization that will be removed.</param>
        /// <response code="204"></response>
        [HttpDelete("", Name = "DeleteRoleAuthorizationByValue")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRoleAuthorizationByValue(int userId, int roleId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userAuths = await _context.RoleAuthorization.Where(a => a.UserId == user.Id).ToListAsync();
            var authToRemove = userAuths.Where(a => a.RoleId == roleId).SingleOrDefault();
            _context.RoleAuthorization.Remove(authToRemove);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
