using FluentValidation;
using KatsumiApp.V1.Application.Features.Following.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KatsumiApp.V1.Application.Features.Following
{
    [ApiController]
    [ControllerName("Following")]
    [Route("api/v{version:apiVersion}/following")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public class FollowController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FollowController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut("follow")]
        [SwaggerOrder(1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FollowUserProfile.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<FollowUserProfile.Result>> AddAsync([FromBody] FollowUserProfile.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
