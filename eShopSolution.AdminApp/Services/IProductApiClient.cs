using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.AdminApp.Services
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request);

    }
}
