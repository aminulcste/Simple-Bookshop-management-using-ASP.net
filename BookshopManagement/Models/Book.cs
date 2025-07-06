namespace BookshopManagement.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        // Foreign key
        public int CategoryId { get; set; }

        // Navigation property (do NOT initialize here)
        public Category? Category { get; set; }
    }
}
