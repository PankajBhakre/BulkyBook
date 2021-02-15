using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if(id == null)
            {
                // This is for create
                return View(category);
            }
            // This is for Edit
            category = _unitofwork.category.Get(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Id ==0)
                {
                    _unitofwork.category.Add(category);
                    
                }
                else
                {
                    _unitofwork.category.Update(category);
                }
                _unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
                return View(category);
            
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitofwork.category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objfromDb = _unitofwork.category.Get(id);
            if(objfromDb == null)
            {
                return Json(new { success = false, message = "Error in Deleting" });
            }
            _unitofwork.category.Remove(objfromDb);
            _unitofwork.Save();
            return Json(new { success = true, message = " Delete Successful" });
        }
    }
}
