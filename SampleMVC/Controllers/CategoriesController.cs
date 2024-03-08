using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

namespace SampleMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryBLL _categoryBLL;

    public CategoriesController(ICategoryBLL categoryBLL)
    {
        _categoryBLL = categoryBLL;
    }

    // public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = "", string act = "")
    // {
    //     if (TempData["message"] != null)
    //     {
    //         ViewData["message"] = TempData["message"];
    //     }

    //     ViewData["search"] = search;
    //     var models = _categoryBLL.GetWithPaging(pageNumber, pageSize, search);
    //     var maxsize = _categoryBLL.GetCountCategories(search);

    //     if (act == "next")
    //     {
    //         if (pageNumber * pageSize < maxsize)
    //         {
    //             pageNumber += 1;
    //         }
    //         ViewData["pageNumber"] = pageNumber;
    //     }
    //     else if (act == "prev")
    //     {
    //         if (pageNumber > 1)
    //         {
    //             pageNumber -= 1;
    //         }
    //         ViewData["pageNumber"] = pageNumber;
    //     }
    //     else
    //     {
    //         ViewData["pageNumber"] = 2;
    //     }

    //     ViewData["pageSize"] = pageSize;

    //     return View(models);
    // }

    public IActionResult Index(int pageNumber = 1, int pageSize = 10, string search = "", string act = "")
    {
        if (TempData["message"] != null)
        {
            ViewData["message"] = TempData["message"];
        }

        ViewData["search"] = search;

        var totalItems = _categoryBLL.GetCountCategories(search);
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        pageNumber = act switch
        {
            "next" when pageNumber < totalPages => pageNumber + 1,
            "prev" when pageNumber > 1 => pageNumber - 1,
            _ => 1,
        };

        ViewData["pageNumber"] = pageNumber;
        ViewData["pageSize"] = pageSize;
        ViewData["totalPages"] = totalPages;

        var models = _categoryBLL.GetWithPaging(pageNumber, pageSize, search);

        return View(models);
    }



    public IActionResult Detail(int id)
    {
        var model = _categoryBLL.GetById(id);
        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(CategoryCreateDTO categoryCreate)
    {
        try
        {
            _categoryBLL.Insert(categoryCreate);
            //ViewData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
        }
        catch (Exception ex)
        {
            //ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var model = _categoryBLL.GetById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, CategoryUpdateDTO categoryEdit)
    {
        try
        {
            _categoryBLL.Update(categoryEdit);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Category Success !</div>";
        }
        catch (Exception ex)
        {
            ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(categoryEdit);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var model = _categoryBLL.GetById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int id, CategoryDTO category)
    {
        try
        {
            _categoryBLL.Delete(id);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Category Success !</div>";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(category);
        }
        return RedirectToAction("Index");
    }

    public IActionResult DisplayDropdownList()
    {
        ViewBag.Categories = _categoryBLL.GetAll();
        return View();
    }

    [HttpPost]
    public IActionResult DisplayDropdownList(string selectedCategoryId)
    {
        ViewBag.SelectedCategoryId = selectedCategoryId;
        ViewBag.Categories = _categoryBLL.GetAll();
        ViewBag.Message = $"You selected {selectedCategoryId}";

        return View();
    }


}
