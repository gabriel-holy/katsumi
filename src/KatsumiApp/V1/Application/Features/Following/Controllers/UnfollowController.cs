using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FluentValidation;
using KatsumiApp.V1.Application.Features.Following.UseCases;

namespace KatsumiApp.V1.Application.Features.Following.Controllers
{
    [ApiController]
    [ControllerName("Following")]
    [Route("api/v{version:apiVersion}/following")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public class UnfollowController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UnfollowController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut("unfollow")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(UnfollowUserProfile.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<UnfollowUserProfile.Result>> UnfollowAsync([FromBody] UnfollowUserProfile.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result is null)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
