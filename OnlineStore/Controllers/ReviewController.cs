using Microsoft.AspNetCore.Mvc;
using OnlineStore.Common;
using OnlineStore.Dtos;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using System.Security.Claims;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet("{reviewID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> GetByIDAsync([FromRoute]int reviewID)
        {

            if (reviewID < 0)
                return new ResponseModel { IsSuccess = false, Message = "ID Musst Be Positive" };
            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reviewService.GetByIDAsync(reviewID , customerID);

            return ReturnResponse(result);
        }

        [HttpPost("update/{reviewID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync([FromRoute]int reviewID,[FromBody] UpdateReviewDTO updatedReview)
        {
            if (reviewID < 0)
                return new ResponseModel { IsSuccess = false, Message = "ID Musst Be Positive" };

            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reviewService.UpdateAsync(reviewID , customerID , updatedReview);

            return ReturnResponse(result);
        }

        [HttpDelete("delete/{reviewID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync([FromRoute] int reviewID)
        {
            if (reviewID < 0)
                return new ResponseModel { IsSuccess = false, Message = "ID Musst Be Positive" };
            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reviewService.DeleteAsync(reviewID, customerID);

            return ReturnResponse(result);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> AddAsync(AddReviewDTO newReview)
        {
            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reviewService.AddAsync(newReview, customerID);

            return ReturnResponse(result);
        }


        private ActionResult<ResponseModel> ReturnResponse<T>(Result<T> result)
        {
            return result.Status switch
            {
                ResultStatus.NotFound => NotFound(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result.ErrorMessage!,
                }),
                ResultStatus.BadRequest => BadRequest(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result.ErrorMessage!,
                }),
                ResultStatus.ServerError => StatusCode(500, new ResponseModel { IsSuccess = false, Message = result.ErrorMessage! }),
                ResultStatus.Success => Ok(new ResponseModel
                {
                    IsSuccess = true,
                    Message = result.ErrorMessage!,
                    Data = result.Data!,
                }),
                _ => StatusCode(500, new { message = "Unknown error" })
            };
        }
    }
}
