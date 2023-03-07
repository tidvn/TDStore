using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TDStore.Models;
using TDStore.Areas.Shop.Models;
using TDStore.Service;

namespace TDStore.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class ShopController : Controller
    {
        private readonly ProductsService _productsService;

        private readonly ImagesService _imagesService;

        public ShopController(ProductsService productsService, ImagesService imagesService)
        {
            _productsService = productsService;
            _imagesService = imagesService;
        }
        public async Task<ActionResult> Index()
        {
            var products = await _productsService.GetAllAsync();
            var list = new List<ProductsView>();
            
            foreach (var product in products)
            {

                List<ImageData> images = new List<ImageData>();
                foreach (var img in product.Images)
                {
                    var imgid = await _imagesService.GetByIdAsync(img.ToString());
                    images.Add(imgid);
                }
                var productsView = new ProductsView
                {
                    Data = product,
                    Image= images

                };
                list.Add(productsView);
            }
            ViewBag.footer = "đây là nội dung footer";
            return View(list);
        }
        public IActionResult Details()
        {
            ViewBag.footer = "đây là nội dung footer";
            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
    }
}
