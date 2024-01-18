﻿
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entites;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
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
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
			var productImage = new ProductImage()
			{
				Caption = request.Caption,
				DateCreated = DateTime.Now,
				IsDefault = request.IsDefault,
				ProductId = productId,
				SortOder = request.SortOrder
			};

			if (request.ImageFile != null)
			{
				productImage.ImagePath = await this.SaveFile(request.ImageFile);
				productImage.FileSize = request.ImageFile.Length;
			}

			_context.ProductImages.Add(productImage);
			await _context.SaveChangesAsync();
            return productImage.Id;
        }
        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
			return await _context.ProductImages.Where(x => x.ProductId == productId)
				.Select(i => new ProductImageViewModel()
				{
					Caption=i.Caption,
					DateCreated = i.DateCreated,
					FileSize = i.FileSize,
					Id = i.Id,
					ImagePath = i.ImagePath,
					IsDefault=i.IsDefault,
					ProductId = i.ProductId,
					SortOder = i.SortOder
				}).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
			var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id{imageId}");
			_context.ProductImages.Remove(productImage);
			return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
			var image = await _context.ProductImages.FindAsync();
            if (image == null)
                throw new EShopException($"Cannot find an image with id{imageId}");


            var viewModel = new ProductImageViewModel()
                {
                    Caption = image.Caption,
                    DateCreated = image.DateCreated,
                    FileSize = image.FileSize,
                    Id = image.Id,
                    ImagePath = image.ImagePath,
                    IsDefault = image.IsDefault,
                    ProductId = image.ProductId,
                    SortOder = image.SortOder
                };
			return viewModel;
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null)
				throw new EShopException($"Cannot find an image with id{imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task AddViewcount(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			product.ViewCount += 1;
			await _context.SaveChangesAsync();
		}

		public async Task<int> Create(ViewModels.Catalog.ProductImages.ProductCreateRequest request)
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

			//Save Image
			if (request.ThumbnailImage != null)
			{
				product.ProductImages = new List<ProductImage>()
				{
					new ProductImage()
					{
						Caption = "Thumbnail Image",
						DateCreated  = DateTime.Now,
						FileSize = request.ThumbnailImage.Length,
						ImagePath = await this.SaveFile(request.ThumbnailImage),
						IsDefault = true,
						SortOder = 1,
                    }
				};
			}
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
			return product.Id;

		}

        public async Task<int> Delete(int productId)
		{
			var product = await _context.Products.FindAsync(productId);

			if (product == null)
				throw new EShopException($"Cannot find a product : {productId}");

            var Images =  _context.ProductImages.Where(i => i.ProductId == productId);
          
			foreach ( var image in Images )
			{
				await _storageService.DeleteFileAsync(image.ImagePath);
			}

			_context.Products.Remove(product);

            //note
            return await _context.SaveChangesAsync();
		}

		public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
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

            if (request.CategoryId.Count > 0)
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

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return productViewModel;

        }

        public async Task<int> Update(ProductUpdateRequest request)
		{
			var product = await _context.Products.FindAsync(request.Id);
			var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
			
			if (product == null || productTranslations == null)
				throw new EShopException($"Cannot find a product with id : {request.Id}");

			productTranslations.Name = request.Name;
			productTranslations.SeoAlias = request.SeoAlias;
			productTranslations.SeoDescription = request.SeoDescription;
			productTranslations.SeoTitle = request.SeoTitle;
			productTranslations.Description = request.Description;
			productTranslations.Details = request.Details;

            //Save Image
            if (request.ThumbnailImage != null)
            {
				var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
				if (thumbnailImage != null)
				{
					thumbnailImage.FileSize = request.ThumbnailImage.Length;
					thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
					_context.ProductImages.Update(thumbnailImage);

                }		
            }

            return await _context.SaveChangesAsync();
		}
        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				throw new EShopException($"Cannot find a product with id : {productId}");
			product.Price = newPrice;
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateStock(int productId, int addedQuantity)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				throw new EShopException($"Cannot find a product with id : {productId}");
			product.Stock += addedQuantity;
			return await _context.SaveChangesAsync() > 0;
		}

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

    }
}
