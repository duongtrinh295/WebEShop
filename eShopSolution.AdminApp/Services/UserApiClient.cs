using eShopSolution.ViewModels.System.Users;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        public UserApiClient() { }
        public Task<string> Authenticate(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
