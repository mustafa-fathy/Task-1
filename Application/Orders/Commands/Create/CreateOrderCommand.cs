using Application.Interfaces;
using Common;
using MediatR;
using Domain.Entities;


namespace Application.Orders.Commands.Create
{
    public class CreateOrderCommand : IRequest<ResponseDto<object>>
    {
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ResponseDto<object>>
        {
            private readonly IAppDbContext _context;
            private readonly ICacheService _cache;

            public CreateOrderHandler(IAppDbContext context, ICacheService cache)
            {
                _context = context;
                _cache = cache;
            }
            public async Task<ResponseDto<object>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {


                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    CustomerName = request.CustomerName,
                    Product = request.Product,
                    Amount = request.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Orders.Add(order);

                await _context.SaveChangesAsync(cancellationToken);

                await _cache.SetAsync($"order:{order.OrderId}", order, TimeSpan.FromMinutes(5));
                await _cache.RemoveAsync("orders:all");

                return ResponseDto<object>.Success(new ResultDto
                {
                    Message = "Created Successfuly ",
                    Result = new
                    {
                        OrderID = order.OrderId,
                    }

                });


            }
        }
    }
}
