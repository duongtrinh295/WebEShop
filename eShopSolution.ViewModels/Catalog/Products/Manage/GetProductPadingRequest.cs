using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products.Manage
{
    public class GetProductPadingRequest : PagingRequestBase
    {
        public string? keyword { get; set; }

        public List<int>? CategoryId { get; set; }
    }
}
