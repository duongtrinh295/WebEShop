﻿
using eShopSolution.Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace eShopSolution.Application.Catalog.Products
{
	public class PublicProductService : IPublicProductService
	{

		public readonly EShopDbContext _context;
		public PublicProductService(EShopDbContext context)
		{
			_context = context;
		}

        public async Task<List<ProductViewModel>> GetAll()
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            
            var data = await query.Select(x => new ProductViewModel
                 {
                     Id = x.p.Id,
                     Name = x.pt.Name,
                     DateCreated = x.p.DateCreated,
                     Description = x.pt.Description,
                     Details = x.pt.Details,
                     LanguageId = x.pt.LanguageId,
                     OriginalPrice = x.p.OriginalPrice,
                     Price = x.p.Price,
                     SeoAlias = x.pt.SeoAlias,
                     SeoDescription = x.pt.SeoDescription,
                     SeoTitle = x.pt.SeoTitle,
                     Stock = x.p.Stock,
                     ViewCount = x.p.ViewCount,
                 }).ToListAsync();

			return data;

        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPadingRequest request)
		{
			// select join
			var query = from p in _context.Products
						join pt in _context.ProductTranslations on p.Id equals pt.ProductId
						join pic in _context.ProductInCategories on p.Id equals pic.ProductId
						join c in _context.Categories on pic.CategoryId equals c.Id
						select new { p, pt, pic };

			// filter
			// sửa lại ròi
			if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
				query = query.Where(p => p.pic.CategoryId == request.CategoryId);

            // pading
            int totalRow = await query.CountAsync();

			var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(x => new ProductViewModel
				{
					Id = x.p.Id,
					Name = x.pt.Name,
					DateCreated = x.p.DateCreated,
					Description = x.pt.Description,
					Details = x.pt.Details,
					LanguageId = x.pt.LanguageId,
					OriginalPrice = x.p.OriginalPrice,
					Price = x.p.Price,
					SeoAlias = x.pt.SeoAlias,
					SeoDescription = x.pt.SeoDescription,
					SeoTitle = x.pt.SeoTitle,
					Stock = x.p.Stock,
					ViewCount = x.p.ViewCount,
				}).ToListAsync();

			// select and projection
			var pageResult = new PagedResult<ProductViewModel>()
			{
				TotalRecord = totalRow,
				Items = data
			};
			return pageResult;
		}
	}
}
