using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PosterrApp.Features.Users
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<ListAllProducts.Result>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<ListAllProducts.Result>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListAllProducts.Result>>> GetAsync()
        {
            var result = await _mediator.Send(new ListAllProducts.Command());

            if (result?.Any() ?? false)
            {
                return Ok(result);
            }

            return NoContent();
        }
    }
}
