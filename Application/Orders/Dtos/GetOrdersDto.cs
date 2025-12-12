namespace Application.Orders.Dtos
{
    public class GetOrdersDto
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
