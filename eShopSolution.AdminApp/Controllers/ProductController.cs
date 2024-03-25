﻿using eShopSolution.AdminApp.Services;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var request = new GetManageProductPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
                Keyword = keyword
            };
            var data = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
                ViewBag.SuccessMsg = TempData["result"];

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }
    }
}
