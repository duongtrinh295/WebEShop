using eShopSolution.Data.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.EF
{
	public class EShopDbContext :DbContext
	{
		public EShopDbContext( DbContextOptions options) : base(options)
		{
			
		}  

		public DbSet<Product> Products { set; get; }
		public DbSet<Category> Categories { set; get; }

	}
}

