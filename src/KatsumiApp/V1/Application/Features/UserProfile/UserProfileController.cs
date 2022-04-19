using KatsumiApp.V1.Application.Features.UserProfile.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KatsumiApp.V1.Application.Features.UserProfile
{

    [ApiController]
    [ControllerName("User Profile")]
    [Route("api/v{version:apiVersion}/profile")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]

    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{username}/viewer/{viewer}")]
        [ProducesResponseType(typeof(ListUserProfile.Result), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ListUserProfile.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<ListUserProfile.Result>> GetAsync(
            [FromRoute] string viewer,
            [FromRoute] string username
        )
        {
            var result = await _mediator.Send(new ListUserProfile.Command()
            {
                Username = username,
                ViewerUsername = viewer,
            });

            if (result != null)
            {
                return Ok(result);
            }

            return NoContent();
        }
    }
}
