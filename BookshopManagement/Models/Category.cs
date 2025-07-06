﻿using System.Collections.Generic;

namespace BookshopManagement.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
