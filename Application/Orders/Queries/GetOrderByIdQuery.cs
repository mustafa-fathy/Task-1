using Application.Interfaces;
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
            public GetOrderByIdHandler(IAppDbContext context)
            {
                _context = context;
            }
            public async Task<ResponseDto<object>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
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
                return ResponseDto<object>.Success(new ResultDto
                {
                    Message = "Order retrieved successfully",
                    Result = order
                });
            }
        }
    }
}
