using KatsumiApp.V1.Application.Features.Post.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KatsumiApp.V1.Application.Features.Post.Controllers
{

    [ApiController]
    [ControllerName("Post")]
    [Route("api/v{version:apiVersion}/post")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public class RegularPostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RegularPostController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut()]
        [SwaggerOrder(1)]
        [ProducesResponseType(typeof(MakeRegularPost.Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MakeRegularPost.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<MakeRegularPost.Result>> PostRegularAsync([FromBody] MakeRegularPost.Command command)
        {
            var result = await _mediator.Send(command);

            if (result != null)
            {
                return Ok(result);
            }

            return NoContent();
        }
    }
}
