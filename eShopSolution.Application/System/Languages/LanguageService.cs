using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using FluentValidation.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _configuration;
        private readonly EShopDbContext _context;

        public LanguageService(IConfiguration configuration, EShopDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {       
            var languages = await _context.Languages.Select(x => new LanguageVm()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();

            return new ApiSuccessResult<List<LanguageVm>>(languages);
        }
    }
}
