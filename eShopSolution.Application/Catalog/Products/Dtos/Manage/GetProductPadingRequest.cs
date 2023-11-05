using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Products.Dtos.Manage
{
	public class GetProductPadingRequest : PagingRequestBase
	{
		public string  keyword { get; set; }

		public List<int>  CategoryId { get; set; }

	}
}
