using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Models.DTO;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public IActionResult Index()
        {
            List<Product> products = _db.Product.Include(p => p.Category).ToList();
            return View(products);
        }

        public async Task<IActionResult> AddProduct()
        {
            if(ViewBag.Categories == null)
            {
                ViewBag.Categories = await _db.Category.ToListAsync();
            }

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            if (ViewBag.Categories == null)
            {
                ViewBag.Categories = await _db.Category.ToListAsync(); //Pre load categories whenever the validation fails
            }

            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image field is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            //Rename image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(productDto.ImageFile!.FileName);

            //Get image path
            string imageFullPath = _environment.WebRootPath + "/images/products/" + newFileName;

            //Save image to path
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await productDto.ImageFile.CopyToAsync(stream);
            }

            //Save product to database
            Product product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CategoryId = productDto.CategoryId,
            };

            _db.Product.Add(product);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
