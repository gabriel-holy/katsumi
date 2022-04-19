using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using KatsumiApp.V1.Application.Features.Post.Quote.UseCases;
using KatsumiApp.V1.Application.Exceptions.Post;

namespace KatsumiApp.V1.Application.Features.Post.Quote
{

    [ApiController]
    [ControllerName("Post")]
    [Route("api/v{version:apiVersion}/post")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]

    public class QuotePostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuotePostController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut("quote/")]
        [ProducesResponseType(typeof(MakeQuotePost.Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MakeQuotePost.Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MakeQuotePost.Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<MakeQuotePost.Result>> PostQuoteAsync([FromBody] MakeQuotePost.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (OriginalPostNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
