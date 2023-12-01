using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetPublicProductPadingRequest : PagingRequestBase
    {
        public string? LanguageId { get; set; }
        public int? CategoryId { get; set; }

    }
}
