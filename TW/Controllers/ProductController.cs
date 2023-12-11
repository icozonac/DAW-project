using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TW.Data;
using TW.Models;
using TW.Models.ViewModels;


namespace TW.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
	        _db = db;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;

            foreach (var obj in objList)
            {
				obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
			}

            return View(objList);
        }


        // GET - Upsert
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new ProductVM()
            {
				Product = new Product(),
				CategorySelectList = _db.Category.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				})
			};

            if (id == null)
            {
				// this is for create
				return View(productVM);
			}
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
					return NotFound();
				}
                return View(productVM);
            }

        }

       //POST - Upsert
       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {

	        if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    // Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream =
                           new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);


                }
                else
                {
                    // Updating

                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            foreach (var modelState in ModelState.Values)
            {
	            foreach (var error in modelState.Errors)
	            {
					Debug.WriteLine(error.ErrorMessage);
				}
            }

			return View(productVM);


        }


        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Category.FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.FirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }



    }
}
