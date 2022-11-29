using EXAMPLE.API.Access.Control.Data;
using EXAMPLE.API.Access.Control.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXAMPLE.API.Access.Control.Controllers
{

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
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
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
