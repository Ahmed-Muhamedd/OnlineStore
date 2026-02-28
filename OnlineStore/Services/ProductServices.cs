using Microsoft.EntityFrameworkCore;
using OnlineStore.Common;
using OnlineStore.Core.Interfaces;
using OnlineStore.Core.Models;
using OnlineStore.Data;
using OnlineStore.Dtos;
using OnlineStore.Interfaces;
using System.Collections;
using System.Linq.Expressions;

namespace OnlineStore.Services
{
    public class ProductServices: IProductService
    {
            // For Test the Repository Pattern //
        //private readonly IBaseRepository<Product> _productRepository;
        //private readonly IBaseRepository<ProductImage> _productImageRepository;
        //private readonly IBaseRepository<ProductCategory> _categoryRepository;
        
        private readonly AppDbContext _context;

        public ProductServices( AppDbContext context) 
        {
            this._context = context;
        }

        // Method helper (not necessary)
        //private async Task<bool> IsExist<T>(Expression<Func<T , bool>> criteria) where T :class
        //{
        //    return await _context.Set<T>().AnyAsync(criteria);
        //}
        public async Task<Result<ProductDto>> AddAsync(CreateProductDto product)
        {

            if(!await _context.ProductCategories.AnyAsync(pk => pk.ID == product.CategoryID))
                return Result<ProductDto>.NotFound("Category ID not found");

            if(await _context.Products.AnyAsync(prd => prd.Name == product.Name))
                return Result<ProductDto>.NotFound("Product name is already exist");

            if (product.ImageUrls == null || !product.ImageUrls.Any())
                return Result<ProductDto>.NotFound("At least one image is required");

            if (product.ImageUrls.Any(url => string.IsNullOrWhiteSpace(url)))
                return Result<ProductDto>.NotFound("Invalid image URL provided");

            if (product.ImageUrls.Count() > 10)
                return Result<ProductDto>.NotFound("Maximum 10 images allowed per product");

            try
            {
                Product newProduct = new Product
                {
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    QuantityInStock = product.QuantityInStock,
                    CategoryID = product.CategoryID,

                    Images = product.ImageUrls.Select(value => new ProductImage { Url = value }).ToList(),
                };
                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();


                return Result<ProductDto>.Success(new ProductDto
                {
                    ProductID = newProduct.ID,
                    CategoryID = newProduct.CategoryID,
                    Description = newProduct.Description,
                    Images = newProduct.Images.Select(p => new ImagesDTO { ImageID = p.ID , ImageUrl = p.Url}),
                    Name = newProduct.Name,
                    Price = newProduct.Price,
                    QuantityInStock = newProduct.QuantityInStock
                });

            }
            catch (Exception ex)
            {
                return Result<ProductDto>.ServerError($"Error adding product: {ex.Message}");
            }

      
        
        }

        public async Task<Result<IEnumerable>> GetAllAsync(Expression<Func<Product, bool>>? criteria = null, bool withInclude = false)
        {
            if (!withInclude)
            {
                var query = _context.Products.AsQueryable();
                if (criteria != null)
                    query = query.Where(criteria);

                var products = await query.ToListAsync();

                //var product = await _productRepository.GetAllAsync();
                if (!products.Any()) 
                    return Result<IEnumerable>.NotFound("No products found");

                return Result<IEnumerable>.Success(products);
            }

            var queryWithInclude = _context.Products.AsQueryable();

            if(criteria != null)
                queryWithInclude = queryWithInclude.Where(criteria);


            var productWithInclude = await queryWithInclude.Select(prd => new
            {
                ProductId = prd.ID,
                ProductName = prd.Name,
                Description = prd.Description,
                Price = prd.Price,
                QuantityInStock = prd.QuantityInStock,
                CategoryName = prd.ProductCategory.Name,
                Images = prd.Images.Select(img => new
                {
                    ImageID = img.ID,
                    ImageUrl = img.Url
                }).ToList()
            }).ToListAsync();

            if (!productWithInclude.Any()) 
                return Result<IEnumerable>.NotFound("No products found");


            return Result<IEnumerable>.Success(productWithInclude);   
        }

        public async Task<Result<PaginationResult>> PaginationAsync(Expression<Func<Product, bool>>? criteria = null, int take = 10, int page = 1)
        {
            if (take < 1) take = 10;
            if (page < 1) page = 1;
            if (take > 100) take = 100;

            var query = _context.Products.AsQueryable();

            if (criteria != null)
                query = query.Where(criteria);

            var count = await query.CountAsync();

            if(count == 0)
               return  Result<PaginationResult>.NotFound("No products found");

            var result = await query.Skip((page - 1) * take).Take(take).ToListAsync();

            return Result<PaginationResult>.Success( new PaginationResult
            {
                TotalItems = count,
                CurrentPage = page,
                PageSize = take,
                TotalPages = (int)Math.Ceiling(count / (double)take),
                Items = result
                
            });
        }

        public async Task<Result<ProductDto>> UpdateAsync(int productID, UpdateProductDto updateProduct)
        {
            if(!await  _context.ProductCategories.AnyAsync(pk => pk.ID == updateProduct.CategoryID))
                return Result<ProductDto>.NotFound($"No Category found with this id: {updateProduct.CategoryID}");

            var product = await _context.Products.Include(image => image.Images)
                .FirstOrDefaultAsync(prd => prd.ID == productID);

            if (product == null) return Result<ProductDto>.NotFound($"No product found with this id: {productID}");

            

            product.Description = updateProduct.Description;
            product.Name = updateProduct.Name;
            product.Price = updateProduct.Price;
            product.QuantityInStock = updateProduct.QuantityInStock;
            product.CategoryID = updateProduct.CategoryID;

            if (updateProduct.Images is not null &&  updateProduct.Images.Any())
            {
               
               foreach (var item in updateProduct.Images)
               {
                    var image = product.Images!.FirstOrDefault(img => img.ID == item.ImageID);
                    if (image == null) return Result<ProductDto>.NotFound($"No image with this ID {item.ImageID}");

                    image.Url = item.ImageUrl;
               }

            }
            try
            {
                await _context.SaveChangesAsync();


                return Result<ProductDto>.Success(new ProductDto
                {
                    ProductID = product.ID,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    CategoryID = product.CategoryID,
                    Images = product.Images!.Select(img => new ImagesDTO { ImageID = img.ID, ImageUrl = img.Url }),
                    QuantityInStock = product.QuantityInStock
                });

            }catch(Exception ex)
            {
                return Result<ProductDto>.ServerError($"Product not updated: {ex.Message}");
            }
      

        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int productID)
        {

            var product = await _context.Products.FindAsync(productID);

            if (product == null) return (false, "Product not found");

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0 ? (true, "Product deleted"): (false, "Failed to delete");
        }

        public async Task<Result<ProductDto>> GetAsync(Expression<Func<Product, bool>> criteria)
        {
            try
            {
                var product = await _context.Products.Include(prd => prd.Images).FirstOrDefaultAsync(criteria);
                if (product is null) return Result<ProductDto>.NotFound("No product Found");

                return Result<ProductDto>.Success(new ProductDto
                {
                    ProductID = product.ID,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    CategoryID = product.CategoryID,
                    Images = product.Images!.Select(img => new ImagesDTO { ImageID = img.ID, ImageUrl = img.Url }),
                    QuantityInStock = product.QuantityInStock
                });

            }
            catch (Exception ex)
            {
                return Result<ProductDto>.ServerError($"Error retrieving product: Unable to connect to database Error: {ex.Message}");
            }
         
        }

        
    }
}
