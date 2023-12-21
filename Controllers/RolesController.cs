using BTL_DOTNET2.Extensions;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_DOTNET2.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class RolesController : Controller
    {
        private CustomUserManager _userManager;
        private readonly RoleManager<IdentityRole> _role;
        public RolesController(RoleManager<IdentityRole> roleManager, CustomUserManager userManager)
        {
            _userManager = userManager;
            _role = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _role.Roles.ToList();
            var _users = _userManager.Users.ToList();
            var roleOfUser = new UserAndRoles
            {
                Users = _users,
                Roles = roles
            };
            return View(roleOfUser);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IdentityRole model)
        {
            if (!_role.RoleExistsAsync(model.Name!).GetAwaiter().GetResult())
            {
                _role.CreateAsync(new IdentityRole(model.Name!)).GetAwaiter().GetResult();
            }
            return View(nameof(Index));
        }
        public IActionResult AddRole()
        {
            var roles = _role.Roles.ToList();
            var _users = _userManager.Users.ToList();

            if (roles != null && _users != null)
            {
                ViewData["Users"] = new SelectList(_users, "Id", "UserName");
                ViewData["Roles"] = new SelectList(roles, "Name", "Name");
                var roleOfUser = new UserAndRoles
                {
                    Users = _users,
                    Roles = roles
                };

                return View(roleOfUser);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(UserAndRoles model)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId!);

                if (user != null && !string.IsNullOrEmpty(model.RoleName))
                {

                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    return RedirectToAction("Index");
                }
            }

            // Nếu có lỗi, quay lại view để hiển thị thông báo hoặc sửa lỗi
            return View(model);
        }
    }
}
