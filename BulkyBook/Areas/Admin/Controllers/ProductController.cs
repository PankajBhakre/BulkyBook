using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitofwork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel vm = new ProductViewModel()
            {
                product = new Product(),
                CategoryList = _unitofwork.category.GetAll().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitofwork.coverType.GetAll().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.NAME,
                    Value = i.ID.ToString()
                })
            };
            if(id == null)
            {
                // This is for create
                return View(vm);
            }
            // This is for Edit
           vm.product = _unitofwork.product.Get(id.GetValueOrDefault());
            if(vm.product == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"Images\Products");
                    var extension = Path.GetExtension(files[0].FileName);
                    if(productViewModel.product.Imageurl != null)
                    {
                        // This is for edit & we need to remove an old image
                        var imagepath = Path.Combine(webRootPath, productViewModel.product.Imageurl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }
                    using(var filestreams = new FileStream(Path.Combine(uploads,filename+extension),FileMode.Create))
                    {
                        files[0].CopyTo(filestreams);
                    }
                    productViewModel.product.Imageurl = @"\Images\Products\\" + filename + extension;
                }
                else
                {
                    // Update when they do not change the image
                    if(productViewModel.product.Id != 0)
                    {
                        Product objfromdb = _unitofwork.product.Get(productViewModel.product.Id);
                        productViewModel.product.Imageurl = objfromdb.Imageurl;
                    }
                }
                if(productViewModel.product.Id == 0)
                {
                    _unitofwork.product.Add(productViewModel.product);
                }
                else
                {
                    _unitofwork.product.Update(productViewModel.product);
                }
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            else
            {
               productViewModel.CategoryList = _unitofwork.category.GetAll().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
               productViewModel.CoverTypeList = _unitofwork.coverType.GetAll().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.NAME,
                    Value = i.ID.ToString()
                });
                
                if(productViewModel.product.Id != 0)
                {
                    productViewModel.product = _unitofwork.product.Get(productViewModel.product.Id);
                }
            }
               return View(productViewModel);
            
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitofwork.product.GetAll(includeProperties:("Category,CoverType"));
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var objfromDb = _unitofwork.product.Get(id);
            if(objfromDb == null)
            {
                return Json(new { success = false, message = "Error in Deleting" });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagepath = Path.Combine(webRootPath, objfromDb.Imageurl.TrimStart('\\'));
            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }
            _unitofwork.product.Remove(objfromDb);
            _unitofwork.Save();
            return Json(new { success = true, message = " Delete Successful" });
        }
    }
}
