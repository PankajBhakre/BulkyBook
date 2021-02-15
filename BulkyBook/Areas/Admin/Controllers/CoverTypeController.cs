using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if(id == null)
            {
                // This is for create
                return View(coverType);
            }
            // This is for Edit
            var parameter = new DynamicParameters();
            parameter.Add("@ID", id);
            coverType = _unitofwork.SP_Call.OneRecord<CoverType>(StaticDetails.Proc_CoverType_Get, parameter);
            //  coverType = _unitofwork.coverType.Get(id.GetValueOrDefault());
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if(ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@NAME", coverType.NAME);
                if(coverType.ID ==0)
                {
                    _unitofwork.SP_Call.Execute(StaticDetails.Proc_CoverType_Create, parameter);
                   // _unitofwork.coverType.Add(coverType);
                    
                }
                else
                {
                    parameter.Add("@ID", coverType.ID);
                    _unitofwork.SP_Call.Execute(StaticDetails.Proc_CoverType_Update, parameter);
                  //  _unitofwork.coverType.Update(coverType);
                }
                _unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
                return View(coverType);
            
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitofwork.SP_Call.List<CoverType>(StaticDetails.Proc_CoverType_GetAll, null);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ID", id);
            var objfromDb = _unitofwork.SP_Call.OneRecord<CoverType>(StaticDetails.Proc_CoverType_Get, parameter);
            if(objfromDb == null)
            {
                return Json(new { success = false, message = "Error in Deleting" });
            }
            _unitofwork.SP_Call.Execute(StaticDetails.Proc_CoverType_Delete, parameter);
           // _unitofwork.coverType.Remove(objfromDb);
            _unitofwork.Save();
            return Json(new { success = true, message = " Delete Successful" });
        }
    }
}
