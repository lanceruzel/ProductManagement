using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, Precision(16, 2)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, MaxLength(100)]
        public string ImageFileName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
