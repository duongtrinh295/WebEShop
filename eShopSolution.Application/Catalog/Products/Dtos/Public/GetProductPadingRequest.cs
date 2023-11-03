using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Products.Dtos.Public
{
	public class GetProductPadingRequest : PagingRequestBase
	{
		public int CategoryId { get; set; }

	}
}
