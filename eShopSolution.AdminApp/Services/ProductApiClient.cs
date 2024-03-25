using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.AdminApp.Services
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        public ProductApiClient(IHttpClientFactory httpClientFactory,
                  IHttpContextAccessor httpContextAccessor,
                   IConfiguration configuration)
           : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ProductVm>>(
                 $"/api/products/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&languageId={request.LanguageId}");
            return data;
        }
    }
}
