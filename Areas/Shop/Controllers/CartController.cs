using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TDStore.Service;
using TDStore.Models;
using TDStore.Helpers;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Buffers.Text;


namespace TDStore.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class CartController : Controller
{
        private readonly ProductsService _productsService;
        private readonly ImagesService _imagesService;
        public CartController(ProductsService productsService, ImagesService imagesService)
        {
            _productsService = productsService;
            _imagesService = imagesService;
        }
        public async Task<IActionResult> Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart");
            
            List<dynamic> list = new List<dynamic>();    

            if (cart != null)
            {
                foreach (var item in cart)
                {
                    var image = await _imagesService.GetByIdAsync(item.Product.Images[0]);
                    dynamic myObject = new System.Dynamic.ExpandoObject();
                    myObject.Product = item.Product;
                    myObject.Image = "data:"+image.FileType+"; base64,"+ @Convert.ToBase64String(image.Data); //; 
                    myObject.Quantity = item.Quantity;
                    myObject.Discount = item.Discount;
                    list.Add(myObject);
                }
                ViewBag.Cart = list;    
                ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity - item.Discount);
            }
            return View();
        }

        public async Task<IActionResult> AddToCart(string id)
        {
            var product = await _productsService.GetByIdAsync(id);
           
            if (product.Quantity <= 0)
                return RedirectToAction("Index", "Shop");

            if (SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart") == null)
            {
                List<ItemOrder> cart = new List<ItemOrder>();
                
                cart.Add(new ItemOrder
                {
                    Product = product,
                    Quantity = 1,
                    Discount = 0
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<ItemOrder> cart = SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ItemOrder
                    {
                        Product = product,
                        Quantity = 1,
                        Discount = 0
                    });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index","Shop");
        }

        public IActionResult RemoveFromCart(string id)
        {
            List<ItemOrder> cart = SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart");
            var item = cart.FirstOrDefault(i => i.Product.Id == id);
            int index = isExist(id);
            if (index != -1 && cart[index].Quantity > 1)
                cart[index].Quantity--;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult subOne(string id)
        {
            List<ItemOrder> cart = SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart");
            int index = isExist(id);
            if (index != -1 && cart[index].Quantity > 1)
                cart[index].Quantity--;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult addOne(string id)
        {
            List<ItemOrder> cart = SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart");
            int index = isExist(id);
            if (index != -1 && cart[index].Quantity < cart[index].Product.Quantity)
                cart[index].Quantity++;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction("Index");
        }
        private int isExist(string id)
        {
            List<ItemOrder> cart = SessionHelper.GetObjectFromJson<List<ItemOrder>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
