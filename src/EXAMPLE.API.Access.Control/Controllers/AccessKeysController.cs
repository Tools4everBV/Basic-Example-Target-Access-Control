using EXAMPLE.API.Access.Control.Data;
using EXAMPLE.API.Access.Control.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXAMPLE.API.Access.Control.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/AccessKeys")]
    [Consumes("application/json")]
    [Produces("application/json")]  
    public class AccessKeysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccessKeysController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


#if DEBUG

        // GET: api/AccessKeys
        /// <summary>
        /// Get all Accesskeys (optional test)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action only for testing this example API
        /// </remarks>
        /// <response code="200"></response>
        [HttpGet(Name = "GetAllAccessKeys")]
        [ProducesResponseType(typeof(List<AccessKey>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<AccessKey>>> GetAccessKey()
        {
            return await _context.AccessKey.ToListAsync();
        }




        // POST: api/AccessKeys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add Accesskey (optional test)
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We will use this action only for testing this example API, generally we do not create access keys with HelloID 
        /// <br></br>
        /// Example:
        ///   
        ///     POST /Accesskeys
        ///     {
        ///        "displayName": "Card001",
        ///        "Type":  "Employee, Visitor or temporary"
        ///        "isActive": "true"
        ///     }
        ///     
        /// </remarks>
        /// <param name="auth">The authorization that will be added.</param>
        /// <response code="201"></response>
        [HttpPost("", Name = "AddAccessKey")]
        [ProducesResponseType(typeof(AccessKey), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AccessKey>> Test_PostAccessKey([FromBody] AccessKey auth)
        {
            _context.AccessKey.Add(auth);
            await _context.SaveChangesAsync();

            return new ObjectResult(auth) { StatusCode = StatusCodes.Status201Created };
        }
#endif

        // PATCH: api/AccessKeys
        /// <summary>
        /// Update AccessKey by Id
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        ///  We will use this action to update an AccessKey. This action does not require a response. A [204 No Content] is sufficient.
        /// 
        /// Example:
        ///   
        ///     PATCH /Users/:id
        ///     [  
        ///         {
        ///             "op": "replace",
        ///             "path": "isActive",
        ///             "value": "true"
        ///         }
        ///     ]
        ///</remarks>
        ///<response code="204"></response>

        [HttpPatch("{id}")]      
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AccessKey>> UpdateAccessKey(int id, [FromBody] JsonPatchDocument<AccessKey> patchDoc)
        {
            var accessKey = await _context.AccessKey.FindAsync(id);
            if (accessKey == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(accessKey, ModelState);

            if (!TryValidateModel(accessKey))
            {
                return BadRequest(ModelState);
            }
            
            await _context.SaveChangesAsync();
            
            return Ok(accessKey);
        }
    }
}
