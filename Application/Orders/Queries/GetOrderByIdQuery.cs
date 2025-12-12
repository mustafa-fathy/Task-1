using Application.Interfaces;
using Application.Orders.Dtos;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<ResponseDto<object>>
    {
        public Guid OrderId { get; set; }
        public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, ResponseDto<object>>
        {
            private readonly IAppDbContext _context;
            private readonly ICacheService _cache;

            public GetOrderByIdHandler(IAppDbContext context, ICacheService cache)
            {
                _context = context;
                _cache = cache;
            }
            public async Task<ResponseDto<object>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
                string cacheKey = $"order:{request.OrderId}";

               
                var cachedOrder = await _cache.GetAsync<GetOrdersDto>(cacheKey);
                if (cachedOrder != null)
                {
                    return ResponseDto<object>.Success(new ResultDto
                    {
                        Message = "Order retrieved successfully (from cache)",
                        Result = cachedOrder
                    });
                }
                var order = await _context.Orders
                    .Where(o => o.OrderId == request.OrderId)
                    .Select(o => new
                    {
                        o.OrderId,
                        o.CustomerName,
                        o.Product,
                        o.Amount,
                        o.CreatedAt
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (order == null)
                {
                    return ResponseDto<object>.Failure(new ErrorDto
                    {
                        Message = "Order not found",
                        Code = 404
                    });
                }


                await _cache.SetAsync(cacheKey, order, TimeSpan.FromMinutes(5));
                return ResponseDto<object>.Success(new ResultDto
                {
                    Message = "Order retrieved successfully",
                    Result = order
                });
            }
        }
    }
}
