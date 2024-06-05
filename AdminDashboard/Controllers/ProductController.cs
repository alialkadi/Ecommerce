using AdminDashboard.Helpers;
using AdminDashboard.Models;
using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController( IUnitOfWork unitOfWork , IMapper mapper )
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var product =await  _unitOfWork.Repository<Product>().GetAllAsync();
            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(product);
            return View(mappedProduct);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                }

                else
                    model.PictureUrl = "images/products/hat-react2.png";

                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                await _unitOfWork.Repository<Product>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");


            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , ProductViewModel model)
        {
            if(id  != model.Id)
                return NotFound();
            if(ModelState.IsValid)
            {
                if( model.Image != null)
                {
                    if(model.PictureUrl != null)
                    {
                        PictureSetting.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");

                    }
                    else
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");

                    var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                    _unitOfWork.Repository<Product>().Update(mappedProduct);

                    var res =await _unitOfWork.CompleteAsync();
                    if(res > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetAsync(id);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            try
            {
                var product = await _unitOfWork.Repository<Product>().GetAsync(id);

                if (product.PictureUrl != null)
                    PictureSetting.DeleteFile(product.PictureUrl, "products");

                _unitOfWork.Repository<Product>().Delete(product);

                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {

                return View(model);
            }
        }
    }
}
