using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Models.DTO
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public IFormFile? ImageFile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int CategoryId { get; set; }
    }
}
