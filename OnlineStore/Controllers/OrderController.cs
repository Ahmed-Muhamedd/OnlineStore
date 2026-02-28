using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "customer")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseModel>> CreateOrderAsync(CreateOrderDto createOrder)
        {
            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _orderService.CreateOrderAsync(createOrder, customerID);

            return ReturnResponse(result);
        }

        [HttpGet("get/{orderID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> GetByIdAsync(int orderID)
        {

            if (orderID < 0) return BadRequest("Order id must be positive"); 

            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _orderService.GetOrderByIdAsync(orderID, customerID);

            return ReturnResponse(result);
        }


        [HttpGet("get/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<ResponseModel>> GetCustomerOrdersAsync()
        {
            var customerID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _orderService.GetCustomerOrdersAsync(customerID);

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
