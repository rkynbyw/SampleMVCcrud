using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using System.Text.Json;
namespace SampleMVC.Controllers;

public class HomeController : Controller
{
	// Home/Index
	public IActionResult Index()
	{

		//check if session not null
		if (HttpContext.Session.GetString("user") != null)
		{
			var userDto = JsonSerializer.Deserialize<UserDTO>(HttpContext.Session.GetString("user"));
			ViewBag.Message = $"Welcome {userDto.FirstName} {userDto.LastName}";
			ViewBag.role = userDto.Roles;
		}
		else
		{
			ViewBag.Message = "Please Login";
		}

		ViewData["Title"] = "Home Page";
		return View();
	}

	public IActionResult Logout()
	{
		HttpContext.Session.Remove("user");
		return RedirectToAction("Login", "Users");
	}

	[Route("/Hello/ASP")]
	public IActionResult HelloASP()
	{
		return Content("Hello ASP.NET Core MVC!");
	}

	// Home/About
	public IActionResult About()
	{
		return View();
	}

	public IActionResult Contact()
	{
		return Content("This is the Contact action method...");
	}
}
