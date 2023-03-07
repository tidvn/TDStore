using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TDStore.Models;
using TDStore.Service;

namespace TDStore.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class ProductsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProductsService _productsService;
        private readonly ImagesService _imagesService;
        private readonly ILogger _logger;
        public ProductsController(
           UserManager<ApplicationUser> userManager,
           ProductsService productsService,
           ImagesService imagesService,
           ILoggerFactory loggerFactory)

        {
            _userManager = userManager;
            _productsService = productsService;
            _imagesService = imagesService;
            _logger = loggerFactory.CreateLogger<ProductsController>();
        }
        public async Task<ActionResult> Index()
        {
            var products = await _productsService.GetAllAsync();

            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, List<IFormFile> files)
        {
           List<String> lst = new List<String>();
            foreach (var file in files)
            {
                var fileModel = new ImageData
                {
                    FileType = file.ContentType,
                    Extension = Path.GetExtension(file.FileName),
                    Title= Path.GetFileNameWithoutExtension(file.FileName)
                };
              using (var dataStream = new MemoryStream())
              {
                   await file.CopyToAsync(dataStream);
                fileModel.Data = dataStream.ToArray();
              }
              await _imagesService.CreateAsync(fileModel);
               lst.Add(fileModel.Id);
            }
            product.Images = new List<String>(lst);
            await _productsService.CreateAsync(product);

            return View();
        }

        [HttpGet, ActionName("Edit")]
        public async Task<ActionResult> Edit(string id)
        {
            var product = await _productsService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = id;
                await _productsService.UpdateAsync(id, product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productsService.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _productsService.GetByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            await _productsService.RemoveAsync(id);

            return RedirectToAction("Index");
        }
    }
}
