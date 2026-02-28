using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OnlineStore.Common;
using OnlineStore.Core.Models;
using OnlineStore.Data;
using OnlineStore.Dtos;
using OnlineStore.Interfaces;

namespace OnlineStore.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManage;
        public ReviewService(AppDbContext context, UserManager<ApplicationUser> userManage)
        {
            _context = context;
            _userManage = userManage;
        }

        public async Task<Result<ReviewResponseDTO>> AddAsync(AddReviewDTO newReview, string customerID)
        {
            //if (!await _context.Products.AnyAsync(p => p.ID == newReview.ProductID))
            //    return Result<ReviewResponseDTO>.NotFound($"Product ID not found {newReview.ProductID}");

            //if (!await _context.Customers.AnyAsync(c => c.UserID == newReview.CustomerID))
            //    return Result<ReviewResponseDTO>.NotFound($"Customer ID not found {newReview.CustomerID}");


            var product = await _context.Products.Where(prd => prd.ID == newReview.ProductID)
                .Select(prd => new { ProductName = prd.Name }).FirstOrDefaultAsync();

            if (product == null)
                return Result<ReviewResponseDTO>.NotFound($"Product ID not found {newReview.ProductID}");

            var Customer = await _userManage.FindByIdAsync(customerID);
            if (Customer == null)

                return Result<ReviewResponseDTO>.NotFound($"Customer ID not found {customerID}");

            var Review = new Review
            {
                CustomerID = customerID,
                Date = DateTime.UtcNow,
                ProductID = newReview.ProductID,
                Rating = newReview.Rating,
                Text = newReview.Text
            };

            try
            {
                _context.Reviews.Add(Review);
                await _context.SaveChangesAsync();


                ReviewResponseDTO? reviewReponse = new ReviewResponseDTO
                {
                    ReviewID = Review.ID,
                    ProductName = product.ProductName,
                    Rating = Review.Rating,
                    ReviewDate = Review.Date,
                    Text = Review.Text,
                    CustomerName = $"{Customer.FirstName} {Customer.SecondName}",
                };

                return Result<ReviewResponseDTO>.Success(reviewReponse);
            }
            catch (Exception ex)
            {
                return Result<ReviewResponseDTO>.ServerError($"Error Occured While Add New Review");
            }
        }

        public async Task<Result<ReviewResponseDTO>> UpdateAsync(int reviewID , string customerID,  UpdateReviewDTO updatedReview )
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(review => review.ID == reviewID
                                                                    && review.CustomerID == customerID );

            if (review == null)
                return Result<ReviewResponseDTO>.NotFound($"Review ID not found {reviewID}");
            review.Text = updatedReview.Text;
            review.Rating = updatedReview.Rating;
            review.Date = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                var response = await _context.Reviews.Where(review => review.ID == reviewID)
                    .Select(review => new ReviewResponseDTO
                    {
                        ReviewID = reviewID,
                        Rating = review.Rating,
                        ReviewDate = review.Date,
                        Text = review.Text,
                        ProductName = review.Product!.Name,
                    }).FirstAsync();

                var customer = await _userManage.FindByIdAsync(customerID);
                response.CustomerName = $"{customer!.FirstName} {customer.SecondName}";

                return Result<ReviewResponseDTO>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<ReviewResponseDTO>.ServerError($"Error Occured While Add New Review");

            }
        }

        public async Task<Result<bool>> DeleteAsync(int ReviewID, string customerID)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(review => review.ID == ReviewID && review.CustomerID == customerID);
            if (review is null) return Result<bool>.NotFound($"Review ID not found {ReviewID}");

            try
            {
                 _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch(Exception ex)
            {
                return Result<bool>.ServerError($"Error occured while delete review data");

            }
        }


        public async Task<Result<ReviewResponseDTO>> GetByIDAsync(int ReviewID, string customerID)
        {
            var response = await _context.Reviews.Where(review => review.ID == ReviewID && review.CustomerID == customerID)
            .Select(review => new ReviewResponseDTO
            {
                ReviewID = review.ID,
                Rating = review.Rating,
                ReviewDate = review.Date,
                Text = review.Text,
                ProductName = review.Product!.Name,
                
            }).FirstOrDefaultAsync();

            if (response is null) return Result<ReviewResponseDTO>.NotFound($"Review ID not found {ReviewID}");

            var customer = await _userManage.FindByIdAsync(customerID);
            response.CustomerName = $"{customer!.FirstName} {customer.SecondName}";



            //var review = await _context.Reviews.FirstOrDefaultAsync(review => review.ID == ReviewID && review.CustomerID == customerID);



            return Result<ReviewResponseDTO>.Success(response);
        }
    }
}