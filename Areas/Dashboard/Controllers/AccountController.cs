using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TDStore.Models;
using System.ComponentModel.DataAnnotations;


namespace TDStore.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    //[Authorize(Roles = "Staff")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Required] string Username, [Required] string Password, string? returnurl = null)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser AppUser = await _userManager.FindByNameAsync(Username);
                if (AppUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(AppUser, Password, false, false);



                    if (result.Succeeded)
                    {

                        // await _userManager.AddClaimAsync(AppUser, claim);


                        _logger.LogInformation("User {UserName} logged in at {Time}.",
                                    AppUser.UserName, DateTime.UtcNow);

                        return Redirect(returnurl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(Username), "Login Failed: Invalid Username or Password");
                        return View();
                    }
                }

            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Redirect("/");
        }

        public IActionResult CreateRole() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole() { Name = name });
                if (result.Succeeded)
                    ViewBag.Message = "Role Created Successfully";
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public IActionResult CreateUser() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Required] string Username, [Required] string Fullname, [Required] string Email, [Required] string Password,string? returnurl = null)
        {
           
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = Username,
                    FullName = Fullname,
                    Email = Email,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, Password);
                await _userManager.AddToRoleAsync(appUser, "Staff");

                if (result.Succeeded)
                    ViewBag.Message = "User Created Successfully";

                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
    }
}
