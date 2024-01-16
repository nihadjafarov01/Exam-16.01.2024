using Exam2.Helpers;
using Exam2.Models;
using Exam2.ViewModels.AuthVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam2.Controllers
{
    public class AuthController : Controller
    {
        UserManager<AppUser> _userManager {  get; }
        SignInManager<AppUser> _signInManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new AppUser
            {
                Email = vm.Email,
                Name = vm.Name,
                Surname = vm.Surname,
                UserName = vm.Username,
            };
            var result = await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            var result = await _signInManager.PasswordSignInAsync(user,vm.Password,true,false);
            if (!result.Succeeded)
            {
                return View(vm);
            }
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<bool> CreateRoles()
        {
            foreach (var item in Enum.GetNames(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item });
                }
            }
            return true;
        }
    }
}
