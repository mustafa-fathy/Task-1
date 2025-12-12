using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
