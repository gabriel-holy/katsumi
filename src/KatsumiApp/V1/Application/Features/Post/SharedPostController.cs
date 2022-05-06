using FluentValidation;
using KatsumiApp.V1.Application.Features.Post.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KatsumiApp.V1.Application.Features.Post
{
    [ApiController]
    [ControllerName("Post")]
    [Route("api/v{version:apiVersion}/post")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public class SharedPostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SharedPostController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut("share/")]
        [SwaggerOrder(2)]
        [ProducesResponseType(typeof(MakeSharedPost.Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MakeSharedPost.Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MakeSharedPost.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<MakeSharedPost.Result>> PostSharedAsync([FromBody] MakeSharedPost.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result != null)
                {
                    return Ok(result);
                }

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
