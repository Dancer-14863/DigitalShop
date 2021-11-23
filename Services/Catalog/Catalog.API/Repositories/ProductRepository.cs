using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Filters;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(CatalogContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Product?>> GetAllFilteredPaginated(int page, int pageSize,
            ProductColumnFilter productColumnFilter)
        {
            IQueryable<Product> products = _table;
            if (productColumnFilter.ProductName != null ||  productColumnFilter.OwnerId!= 0)
            {
                products = products.Where(product =>
                    (productColumnFilter.ProductName == null || product.ProductName.Contains(productColumnFilter.ProductName)) &&
                    (productColumnFilter.OwnerId == 0 || product.OwnerId == productColumnFilter.OwnerId));
            }

            return await products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}