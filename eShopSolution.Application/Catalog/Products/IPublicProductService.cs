
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products.Public;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
         Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPadingRequest request);
      
    }
}
