﻿
using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Manage;
using eShopSolution.Application.Dtos;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entites;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
	public class ManageProductService : IManageProductService
	{
		public readonly EShopDbContext _context;
		public ManageProductService(EShopDbContext context)
		{
			_context = context;
		}

		public async Task AddViewcount(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			product.ViewCount += 1;
			await _context.SaveChangesAsync();
		}

		public async Task<int> Create(ProductCreateRequest request)
		{
			var product = new Product()
			{
				Price = request.Price,
				OriginalPrice = request.OriginalPrice,
				Stock = request.Stock,
				ViewCount = 0,
				DateCreated = DateTime.Now,
				ProductTranslations = new List<ProductTranslation>()
				{
					new ProductTranslation()
					{
						Name = request.Name,
						Description = request.Description,
						Details = request.Details,
						SeoDescription = request.SeoDescription,
						SeoAlias = request.SeoAlias,
						SeoTitle = request.SeoTitle,
						LanguageId = request.LanguageId,
					}
				}
			};
			_context.Products.Add(product);
			return await _context.SaveChangesAsync();

		}

		public async Task<int> Delete(int productId)
		{
			var product = await _context.Products.FindAsync(productId);

			if (product == null)
				throw new EShopException($"Cannot find a product : {productId}");
			
			_context.Products.Remove(product);
			return await _context.SaveChangesAsync();
		}

		public Task<List<ProductViewModel>> GetAll()
		{
			throw new NotImplementedException();
		}

		public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPadingRequest request)
		{
			// select join
			var query = from p in _context.Products
						join pt in _context.ProductTranslations on p.Id equals pt.ProductId
						join pic in _context.ProductInCategories on p.Id equals pic.ProductId
						join c in _context.Categories on pic.CategoryId equals c.Id
						select new { p, pt , pic};

			// filter
			if (!string.IsNullOrEmpty(request.keyword))
				query = query.Where(x => x.pt.Name.Contains(request.keyword));

			if (request.CategoryId.Count >0)
				query = query.Where(p => request.CategoryId.Contains(p.pic.CategoryId));

			// pading
			int totalRow = await query.CountAsync();

			var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(x=> new ProductViewModel 
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
		 

		public async Task<int> Update(ProductUpdateRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdatePrice(int productId, decimal newPrice)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateStock(int productId, int addedQuantity)
		{
			throw new NotImplementedException();
		}
	}
}
