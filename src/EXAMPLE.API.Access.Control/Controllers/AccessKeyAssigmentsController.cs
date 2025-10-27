
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
    public class AccessKeyAssignmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccessKeyAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/accesskeyassignments
        /// <summary>
        /// Get AccessKeyAssignments for all users (optional)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This can be userful for reporting, but is generally not required for HelloID because the Accesskeys are typically not assigned by HelloId
        /// </remarks>
        /// <returns>List of all AccessKeyAssignments</returns>
        /// <response code="200">Returns the list of all AccessKeyAssignments</response>
        /// <response code="401">Authentication required or failed.</response>
        [HttpGet("", Name = "GetAllAccessKeyAssignments")]
        [ProducesResponseType(typeof(List<AccessKeyAssignment>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<AccessKeyAssignment>>> GetAccessKeyAssignment()
        {
            var Assignments = await _context.AccessKeyAssignment.ToListAsync();
            return Ok(Assignments);
        }


        // GET: api/AccessKeyAssignments/user/{userId}
        /// <summary>
        /// Get all assigned Access keys for a specific user
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// This returns all Access Keys currently assigned to the given user.
        /// <br></br>
        /// Useful when validating which permissions/roles the user has at a given moment.
        /// </remarks>
        /// <response code="200">Returns a list of AccessKeyAssignments</response>
        /// <response code="404">If the user does not exist</response>
        /// <response code="401">Authentication required or failed.</response>
        [HttpGet("{userId:int}/Accesskeys", Name = "GetAccessKeysByUserId")]
        [ProducesResponseType(typeof(List<AccessKey>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<AccessKey>>> GetAccessKey(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var accessKeysAssignments = await _context.AccessKeyAssignment.Where(a => a.UserId == userId).ToListAsync();
            var accessKeys = _context.AccessKey.Where(a => accessKeysAssignments.Select(a => a.Id).Contains(a.Id));

            return Ok(accessKeys);
        }

#if DEBUG

        // POST: api/AccessKeyAssignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add AccessKeyAssignment (optional test)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action only for testing this example API, generally we do not assign access keys to users in helloID 
        /// <br></br>
        /// Example:
        ///   
        ///     POST /accesskeyassignments
        ///     {
        ///        "accessKeyId": 1,
        ///        "userId": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="auth">The authorization that will be added.</param>
        /// <response code="201"></response>
        [HttpPost("", Name = "AddAccessKeyAssignment")]
        [ProducesResponseType(typeof(AccessKeyAssignment), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AccessKeyAssignment>> Test_PostAccessKeyAssignment([FromBody] AccessKeyAssignment auth)
        {
            _context.AccessKeyAssignment.Add(auth);
            await _context.SaveChangesAsync();

            return new ObjectResult(auth) { StatusCode = StatusCodes.Status201Created };
        }
#endif
        // DELETE: api/AccessKeyAssignment/:id
        /// <summary>
        /// Delete AccessKeyAssignment (by Id)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action when an AccessKey is revoked from a user. This action does not require a response. A [204 No Content] is sufficient.
        /// </remarks>
        /// <param name="auth">The authorization that will be removed.</param>
        /// <response code="204">The authorization was successfully removed.</response>
        /// <response code="404">No authorization with the specified Id was found.</response>
        /// <response code="401">Authentication required or failed.</response>
        [HttpDelete("{id}", Name = "DeleteAccessKeyAssignment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccessKeyAssignment(int id)
        {
            var auth = await _context.AccessKeyAssignment.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            _context.AccessKeyAssignment.Remove(auth);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/AccessKeyAssignment/&userid=:id;&AccessKeyId=:id
        /// <summary>
        /// Delete AccessKeyAssignment
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action when an AccessKey is revoked from a user. This action does not require a response. A [204 No Content] is sufficient.
        /// </remarks>
        /// <param name="auth">The authorization that will be removed.</param>
        /// <response code="204"></response>
        [HttpDelete("", Name = "DeleteAccessKeyAssignmentByValue")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccessKeyAssignmentByValue(int userId, int accessKeyId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userAuths = await _context.AccessKeyAssignment.Where(a => a.UserId == user.Id).ToListAsync();
            var authToRemove = userAuths.Where(a => a.AccessKeyId == accessKeyId).SingleOrDefault();
            _context.AccessKeyAssignment.Remove(authToRemove);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
