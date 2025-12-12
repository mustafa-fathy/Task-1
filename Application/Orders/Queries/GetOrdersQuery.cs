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
            public GetOrdersHandler(IAppDbContext context)
            {
                _context = context;
            }
            public async Task<ResponseDto<object>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                var pagenumber = request.PageNumber <= 0 ? 1 : request.PageNumber;

                var pageSize = 10;
                var query = _context.Orders.AsNoTracking();

                var totalCount = await query.CountAsync(cancellationToken);

                var totalPages = (totalCount + pageSize - 1) / pageSize;

                var orders = await query
                    .Select(o => new GetOrdersDto
                    {
                        OrderId = o.OrderId,
                        Amount = o.Amount,
                        CreatedAt = DateTime.Now,
                        CustomerName = o.CustomerName,
                        Product = o.Product,

                    }).ToListAsync(cancellationToken);

                var paginatedlist = new PaginatedList<GetOrdersDto>
                {
                    Items = orders,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    PageNumber = pagenumber,

                };

                return ResponseDto<object>.Success(new ResultDto
                {
                    Message = "All Orders ",
                    Result = new
                    {
                        paginatedlist
                    }
                });
            }
        }
    }
}
