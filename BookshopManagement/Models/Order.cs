using System;
using System.Collections.Generic;

namespace BookshopManagement.Models
{
    public class Order
    {
        public int OrderId { get; set; }



 public string? UserId { get; set; }          // Nullable
        public ApplicationUser? User { get; set; }   // Nullable

        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending";
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }


}
