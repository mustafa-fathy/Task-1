using Application.Interfaces;
using Application.Orders.Dtos;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries
{
    public class GetOrdersQuery : IRequest<ResponseDto<object>>
    {
        public int PageNumber { get; set; } = 1;

        public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, ResponseDto<object>>
        {
            private readonly IAppDbContext _context;
            private readonly ICacheService _cache;
            private const int PageSize = 10;

            public GetOrdersHandler(IAppDbContext context, ICacheService cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<ResponseDto<object>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
                string cacheKey = $"orders:all:page:{pageNumber}:size:{PageSize}";

                
                var cachedOrders = await _cache.GetAsync<PaginatedList<GetOrdersDto>>(cacheKey);
                if (cachedOrders != null)
                {
                    return ResponseDto<object>.Success(new ResultDto
                    {
                        Message = "Orders fetched successfully (from cache)",
                        Result = cachedOrders
                    });
                }

                
                var query = _context.Orders.AsNoTracking();

                var totalCount = await query.CountAsync(cancellationToken);
                var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

                var orders = await query
                    .Select(o => new GetOrdersDto
                    {
                        OrderId = o.OrderId,
                        CustomerName = o.CustomerName,
                        Product = o.Product,
                        Amount = o.Amount,
                        CreatedAt = o.CreatedAt
                    })
                    .ToListAsync(cancellationToken);

                var paginatedList = new PaginatedList<GetOrdersDto>
                {
                    Items = orders,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    PageSize = PageSize,
                    PageNumber = pageNumber
                };

                
                await _cache.SetAsync(cacheKey, paginatedList, TimeSpan.FromMinutes(5));

                return ResponseDto<object>.Success(new ResultDto
                {
                    Message = "Orders fetched successfully",
                    Result = paginatedList
                });
            }
        }
    }
}
