using ProductManagement.Models.DTO;

namespace ProductManagement.Models
{
    public class AddProductViewModel
    {
        public ProductDto Product { get; set; } = new ProductDto();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
