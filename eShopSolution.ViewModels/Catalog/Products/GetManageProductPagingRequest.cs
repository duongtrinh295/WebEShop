﻿using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.ProductImages
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string? keyword { get; set; }

        public List<int>? CategoryId { get; set; }
    }
}
