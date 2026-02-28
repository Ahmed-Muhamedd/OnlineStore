using Microsoft.AspNetCore.Components.Web;
using OnlineStore.Common;
using OnlineStore.Core.Models;
using OnlineStore.Dtos;

namespace OnlineStore.Interfaces
{
    public interface IReviewService
    {
        Task<Result<ReviewResponseDTO>> AddAsync(AddReviewDTO review ,string customerID);
        Task<Result<ReviewResponseDTO>> UpdateAsync(int reviewID, string customerID, UpdateReviewDTO updatedReview);
        Task<Result<bool>> DeleteAsync(int ReviewID, string customerID);
        Task<Result<ReviewResponseDTO>> GetByIDAsync(int ReviewID, string customerID);
    }
}
