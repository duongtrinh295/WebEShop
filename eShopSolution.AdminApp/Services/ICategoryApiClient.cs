using eShopSolution.ViewModels.Catalog.Categories;

namespace eShopSolution.AdminApp.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAll(string languageId);

    }
}
