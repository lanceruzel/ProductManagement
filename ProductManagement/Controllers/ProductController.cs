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

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Product.Include(p => p.Category).ToListAsync();
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

        public async Task<IActionResult> EditProduct(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = await _db.Product.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            if (ViewBag.Categories == null)
            {
                ViewBag.Categories = await _db.Category.ToListAsync();
            }

            var productDto = new ProductDto
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId
            };

            //Get current image file name
            ViewData["ImageFileName"] = product.ImageFileName;

            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(int? id, ProductDto productDto)
        {
            if (id == null || id == 0 || productDto == null)
            {
                return NotFound();
            }

            Product? product = await _db.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["ImageFileName"] = product.ImageFileName;
                return View(productDto);
            }

            //Get current file name
            string newFileName = product.ImageFileName;

            //Check if there is new image file
            if (productDto.ImageFile != null)
            {
                //Rename image file
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(productDto.ImageFile.FileName);

                //Get image path
                string imageFullPath = _environment.WebRootPath + "/images/products/" + newFileName;

                //Save image to path
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await productDto.ImageFile.CopyToAsync(stream);
                }

                //Delete current image file
                string oldImageFullPath = _environment.WebRootPath + "/images/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            //Update product
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFileName = newFileName;
            product.CategoryId = productDto.CategoryId;

            _db.Update(product);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
