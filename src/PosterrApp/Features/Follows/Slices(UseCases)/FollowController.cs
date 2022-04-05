using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PosterrApp.Features.Follows;
using System;
using System.Threading.Tasks;

namespace VerticalApp.Features.Follows
{
    [ApiController]
    [Route("products/{productId}/")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public class FollowController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FollowController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("add-stocks")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(AddFollow.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<AddFollow.Result>> AddAsync(int productId, [FromBody] AddFollow.Command command)
        {
            command.ProductId = productId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("remove-stocks")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RemoveFollow.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<RemoveFollow.Result>> RemoveAsync(int productId, [FromBody] RemoveFollow.Command command)
        {
            try
            {
                command.ProductId = productId;
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (CantFollowYourselfException ex)
            {
                return Conflict(new
                {
                    ex.Message,
                    ex.AmountToRemove,
                    ex.QuantityInStock
                });
            }
        }
    }
}
