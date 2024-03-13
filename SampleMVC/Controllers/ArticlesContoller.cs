using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text.Json;

namespace SampleMVC.Controllers;

public class ArticlesController : Controller
{
    // private readonly IArticleBLL _articleBLL;

    // public ArticlesController(IArticleBLL articleBLL)
    // {
    //     _articleBLL = articleBLL;
    // }

    private readonly IArticleBLL _articleBLL;
    private readonly ICategoryBLL _categoryBLL;

    public ArticlesController(IArticleBLL articleBLL, ICategoryBLL categoryBLL)
    {
        _articleBLL = articleBLL;
        _categoryBLL = categoryBLL;
    }

    public IActionResult Index(int pageNumber = 1, int pageSize = 7, string search = "", string act = "", int? categoryId = null)
    {
        if (HttpContext.Session.GetString("user") != null)
        {
            var userDto = JsonSerializer.Deserialize<UserDTO>(HttpContext.Session.GetString("user"));
            ViewBag.role = userDto.Roles;
        }
        else
        {
            ViewBag.Message = "Please Login";
        }

        if (TempData["message"] != null)
        {
            ViewData["message"] = TempData["message"];
        }

        ViewData["search"] = search;
        ViewData["selectedCategoryId"] = categoryId;

        // Mendapatkan daftar kategori
        var categories = _categoryBLL.GetAll();  // Sesuaikan dengan metode yang Anda miliki
        ViewBag.Categories = categories;

        var totalItems = categoryId.HasValue
            ? _articleBLL.GetCountArticleByCategory(categoryId.Value, search)
            : _articleBLL.GetCountArticle(search);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        if (act == "next" && pageNumber < totalPages)
        {
            pageNumber += 1;
        }
        else if (act == "prev" && pageNumber > 1)
        {
            pageNumber -= 1;
        }

        ViewData["pageNumber"] = pageNumber;
        ViewData["pageSize"] = pageSize;
        ViewData["totalPages"] = totalPages;

        IEnumerable<ArticleDTO> models;
        // int categoryId, int pageNumber, int pageSize, string search
        if (categoryId.HasValue)
        {
            models = _articleBLL.GetArticleByCategoryPaging(categoryId.Value, pageNumber, pageSize, search);
        }
        else
        {
            models = _articleBLL.GetWithPaging(pageNumber, pageSize, search);
        }

        return View(models);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ArticleCreateDTO articleCreate, IFormFile imageArticle)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            if (imageArticle != null && imageArticle.Length > 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageArticle.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picts", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageArticle.CopyTo(stream);
                }

                articleCreate.Pic = fileName;
            }

            _articleBLL.Insert(articleCreate);

            TempData["message"] = "<div class='alert alert-success'><strong>Success!</strong> Add Data Category Success !</div>";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
        }

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {

        var model = _articleBLL.GetArticleById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, ArticleUpdateDTO articleUpdate, IFormFile imageArticle)
    {
        try
        {
            if (imageArticle != null && imageArticle.Length > 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageArticle.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picts", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageArticle.CopyTo(stream);
                }

                articleUpdate.Pic = fileName;
            }
            _articleBLL.Update(articleUpdate);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Article Success !</div>";
        }
        catch (Exception ex)
        {
            ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(articleUpdate);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var model = _articleBLL.GetArticleById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int id, ArticleDTO article)
    {
        try
        {
            _articleBLL.Delete(id);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Category Success !</div>";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(article);
        }
        return RedirectToAction("Index");
    }


}