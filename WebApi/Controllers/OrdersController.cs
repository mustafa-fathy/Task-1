using Application.Orders.Commands.Create;
using Application.Orders.Commands.Delete;
using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Error);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Error);
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1)
        {
            var query = new GetOrdersQuery { PageNumber = pageNumber };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Error);
        }
        [HttpGet("fetch1")]
        public async Task<IActionResult> GetbyId(Guid id)
        {
            var command = new DeleteOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Error);
        }



    }
}
