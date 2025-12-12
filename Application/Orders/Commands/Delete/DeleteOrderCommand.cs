using Application.Interfaces;
using Common;
using MediatR;
using SendGrid.Helpers.Errors.Model;

namespace Application.Orders.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<ResponseDto<object>>
    {
        public Guid OrderId { get; set; }
        public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, ResponseDto<object>>
        {
            private readonly IAppDbContext _context;
            private readonly ICacheService _cache;
            public DeleteOrderHandler(IAppDbContext context, ICacheService cache)
            {
                _context = context;
                _cache = cache;
            }
            public async Task<ResponseDto<object>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {

                var order = await _context.Orders.FindAsync(request.OrderId, cancellationToken) ?? throw new NotFoundException("Order not found");
                order.Deleted = true;// Soft Delete "recommended" here we can make it back if needed....i've made a configuration for order entity.
                // _context.Orders.Remove(order);//Hard Delete "not recommended"
                await _context.SaveChangesAsync(cancellationToken);

                await _cache.RemoveAsync($"order:{request.OrderId}");
                await _cache.RemoveAsync("orders:all");

                return ResponseDto<object>.Success(new ResultDto
                {
                    Message = "Deleted Successfully",
                    Result = new
                    {
                        OrderID = order.OrderId,
                    }
                });
            }
        }
    }
}
