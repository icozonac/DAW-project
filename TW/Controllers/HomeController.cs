using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TW.Data;
using TW.Models;
using TW.Models.ViewModels;

namespace TW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).ToList(),
				Categories = _db.Category
			};  

            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get(WC.SessionCart) is byte[] byteArray && byteArray.Length > 0)
            {
                shoppingCartList = JsonConvert.DeserializeObject<List<ShoppingCart>>(Encoding.UTF8.GetString(byteArray)) ?? new List<ShoppingCart>();
            }


            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType)
                    .FirstOrDefault(u => u.Id == id),
                ExistsInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }

            return View(detailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get(WC.SessionCart) is byte[] byteArray && byteArray.Length > 0)
            {
                shoppingCartList = JsonConvert.DeserializeObject<List<ShoppingCart>>(Encoding.UTF8.GetString(byteArray)) ?? new List<ShoppingCart>();
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shoppingCartList)));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get(WC.SessionCart) is byte[] byteArray && byteArray.Length > 0)
            {
                shoppingCartList = JsonConvert.DeserializeObject<List<ShoppingCart>>(Encoding.UTF8.GetString(byteArray)) ?? new List<ShoppingCart>();
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);

            if (itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shoppingCartList)));

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}