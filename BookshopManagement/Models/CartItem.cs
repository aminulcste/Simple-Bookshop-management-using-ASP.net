namespace BookshopManagement.Models
{
    public class CartItem
    {
        public int BookId { get; set; }
        public Book? Book { get; set; } // Nullable to avoid null assignment warning
        public int Quantity { get; set; }
    }
}
