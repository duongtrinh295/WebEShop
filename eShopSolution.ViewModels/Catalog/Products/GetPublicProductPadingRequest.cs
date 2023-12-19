using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.ProductImages
{
    public class GetPublicProductPadingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }

    }
}
