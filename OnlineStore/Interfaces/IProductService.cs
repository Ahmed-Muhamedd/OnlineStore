using OnlineStore.Common;
using OnlineStore.Core.Interfaces;
using OnlineStore.Core.Models;
using OnlineStore.Dtos;
using System.Collections;
using System.Linq.Expressions;

namespace OnlineStore.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto>> AddAsync(CreateProductDto product);
        Task<Result<ProductDto>> UpdateAsync(int productID , UpdateProductDto updateProduct);

        Task<Result<IEnumerable>> GetAllAsync(Expression<Func<Product , bool>>? criteria = null, bool withInclude = false);
        Task<Result<ProductDto>> GetAsync(Expression<Func<Product , bool>> criteria );
        Task<Result<PaginationResult>> PaginationAsync(Expression<Func<Product, bool>>? criteria = null, int take = 10, int page = 1 );
        Task<(bool IsDeleted, string Message)> DeleteAsync(int productID);
    }
}
