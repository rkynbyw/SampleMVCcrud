using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text.Json;

namespace SampleMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserBLL _userBLL;
        private readonly IRoleBLL _roleBLL;
        public UsersController(IUserBLL userBLL, IRoleBLL roleBLL)
        {
            _userBLL = userBLL;
            _roleBLL = roleBLL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUserWithRoles()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var userDto = _userBLL.LoginMVC(loginDTO);
                //simpan username ke session
                var userDtoSerialize = JsonSerializer.Serialize(userDto);
                HttpContext.Session.SetString("user", userDtoSerialize);

                TempData["Message"] = "Welcome " + userDto.Username;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Profile()
        {
            var userDtoSerialized = HttpContext.Session.GetString("user");
            var userDto = JsonSerializer.Deserialize<UserDTO>(userDtoSerialized);

            return View(userDto);
        }

        public IActionResult UserProfileRole()
        {
            var userDtoSerialized = HttpContext.Session.GetString("user");
            var userDto = JsonSerializer.Deserialize<UserDTO>(userDtoSerialized);
            var currentUserName = userDto.Username;

            try
            {
                var userWithRoles = _userBLL.GetUserWithRoles(currentUserName);
                return View(userWithRoles);
            }
            catch (ArgumentException ex)
            {
                ViewBag.Message = $"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>{ex.Message}</div>";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>{ex.Message}</div>";
                return View();
            }
        }

        public IActionResult ListUser()
        {
            var users = _userBLL.GetAll();
            var listUsers = new SelectList(users, "Username", "Username");
            ViewBag.Users = listUsers;

            var roles = _roleBLL.GetAllRoles();
            var listRoles = new SelectList(roles, "RoleID", "RoleName");
            ViewBag.Roles = listRoles;

            //var userWithRoles = _userBLL.GetUserWithRoles("ekurniawan");
            var usersWithRoles = _userBLL.GetAllWithRoles();
            return View(usersWithRoles);
        }

        [HttpPost]
        public IActionResult AddRoleToUser(string username, int roleId)
        {
            try
            {
                _roleBLL.AddUserToRole(username, roleId);
                TempData["Message"] = $"Role added successfully to user {username}.";
            }
            catch (ArgumentException ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ListUser");
        }

    }
}