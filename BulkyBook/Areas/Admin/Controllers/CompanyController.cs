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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if(id == null)
            {
                // This is for create
                return View(company);
            }
            // This is for Edit
            company = _unitofwork.company.Get(id.GetValueOrDefault());
            if(company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company  company)
        {
            if(ModelState.IsValid)
            {
                if(company.Id ==0)
                {
                    _unitofwork.company.Add(company);
                    
                }
                else
                {
                    _unitofwork.company.Update(company);
                }
                _unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
                return View(company);
            
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitofwork.company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objfromDb = _unitofwork.company.Get(id);
            if(objfromDb == null)
            {
                return Json(new { success = false, message = "Error in Deleting" });
            }
            _unitofwork.company.Remove(objfromDb);
            _unitofwork.Save();
            return Json(new { success = true, message = " Delete Successful" });
        }
    }
}
